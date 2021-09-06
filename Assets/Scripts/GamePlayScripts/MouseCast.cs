using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCast : MonoBehaviour
{
    RaycastHit _hit;
    private GameObject _currentTarget;
   
   
    void Update()
    {
        if (Input.GetMouseButtonDown(1) &&
                 Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit))
        {
            if (_hit.collider.tag == "Unit")
            {
                _currentTarget = _hit.collider.gameObject;
                _currentTarget.GetComponent<Renderer>().material.color = Color.red;
            }
            else if (_hit.collider.tag == "GreenPart" && _currentTarget != null)
            {
                _currentTarget.transform.TransformDirection(_hit.transform.position);
            }
            else if (_hit.collider.tag == "GreenPart" && _currentTarget == null)
            {
                return;
            }
            else return;
        }
    }
}
