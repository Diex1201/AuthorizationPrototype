using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCast : MonoBehaviour
{
    RaycastHit _hit;
    public LayerMask _clicableLayer;
    public Material _defaultMaterial;
    public Material _changeMaterial;
    private GameObject _currentTarget;


    void Update()
    {
        if (Input.GetMouseButtonDown(1) &&
                 Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit, _clicableLayer))
        {
            if (_hit.collider.tag == "Unit")
            {
                _currentTarget = _hit.collider.gameObject;
                _currentTarget.GetComponent<MeshRenderer>().material = _changeMaterial;
            }
        }
    }
}
