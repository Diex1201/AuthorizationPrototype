using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewUserCreate;
using NativeWebSocket;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[Serializable]
  class ErrServer
{
  public int code;
  public string message;
}
[Serializable]
     class ResultServer
     {
        public bool auth;
     }

   [Serializable]
     class AnswerFromServer
    {
        public string id;
        public ErrServer err;
        public ResultServer result;
    }
    public class Connection : MonoBehaviour
{
    WebSocket websocket;
    public Text _notificationText;
    AccountDataInfo _newAccount;
    private bool _requestIsSent;
    

    async void Start()
    {
        websocket = new WebSocket("wss://srv2.warforgalaxy.com:9982");
        _newAccount = FindObjectOfType<AccountDataInfo>();

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
            _notificationText.text = "Connection open!";
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
            _notificationText.text = "Error! " + e;
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            AnswerFromServer ans = new AnswerFromServer();
            ans = JsonUtility.FromJson<AnswerFromServer>(message);
            if(ans.result.auth && ans.id == _newAccount._idUser)
            {
                _notificationText.text = "Авторизация прошла успешно!";
                StartCoroutine(LoadGameScene());
            }
           else if(ans.err.code == -32040 && ans.err.message == "Login or password are invalid")
            {
                _notificationText.text = "Неверный логин или пароль";
            }
            else _notificationText.text = "Неверный логин или пароль";
            Debug.Log("OnMessage! " + message);
        };
        InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    
  async  public void Authorization()
    {
        await websocket.Connect();
    }
    async public void SendWebSocketMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            if(!_requestIsSent)
            {
                await websocket.SendText(_newAccount._dataAccount);
                _requestIsSent = true;
            }
            // Sending bytes
            //await websocket.Send(new byte[] { 10, 20, 30 });
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

    IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("GameScene");
    }

}

