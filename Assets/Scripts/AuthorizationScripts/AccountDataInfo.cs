//using Newtonsoft.Json;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace NewUserCreate
{
    [Serializable]
    class Params
    {
        public string login;
        public string pass;
    }
    [Serializable]
    class NewUser
    {
        public string id;
        public string jsonrpc;
        public string method;
        public Params param;

        public string SavetoJson()
        { return JsonUtility.ToJson(this); }
    }
    public class AccountDataInfo : MonoBehaviour
    {
        public InputField _login;
        public InputField _pass;
        public Connection _connection;
        public string _dataAccount;
        public string _idUser;

        public void AuthorizationOn()
        {
            _connection.Authorization();
            NewUser _newUser = new NewUser
            {
                id = Guid.NewGuid().ToString(),
                jsonrpc = "2.0",
                method = "auth",
                param = new Params { login = _login.text, pass = GetHash(_pass.text).ToUpper() }
               };
        String str=JsonUtility.ToJson(_newUser);
        _dataAccount = str.Replace("param", "params");
            _idUser = _newUser.id;
            Debug.Log(_dataAccount);
        }

        private string GetHash(string _input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(_input));
            return BitConverter.ToString(hash).Replace("-", String.Empty);
        }
    }
}
