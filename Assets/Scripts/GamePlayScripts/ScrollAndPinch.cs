using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollAndPinch : MonoBehaviour
{
#if UNITY_ANDROID

    [SerializeField] private Camera _cam;
    [SerializeField] private bool _rotate;
    protected Plane _plane;
    void Awake()
    {
        if (_cam == null) _cam = Camera.main;
    }


    void Update()
    {
        if(Input.touchCount >= 1)
        {
            _plane.SetNormalAndPosition(transform.up, transform.position);
        }

        var delta1 = Vector3.zero;
        var delta2 = Vector3.zero;

        if(Input.touchCount >= 1)
        {
            delta1 = PlanePositionDelta(Input.GetTouch(0));
            if(Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                _cam.transform.Translate(delta1, Space.World);
            }
        }

        if(Input.touchCount >= 2)
        {
            var pos1 = PlanePosition(Input.GetTouch(0).position);
            var pos2 = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            var zoom = Vector3.Distance(pos1, pos2) / Vector3.Distance(pos1b, pos2b);
            if(zoom == 0 || zoom > 10)
            {
                return;
            }

            _cam.transform.position = Vector3.LerpUnclamped(pos1, _cam.transform.position, 1 / zoom);

            if(_rotate && pos2b != pos2)
            {
                _cam.transform.RotateAround(pos1, _plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b,
                    _plane.normal));
            }
        }
    }

    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        var _rayNow = _cam.ScreenPointToRay(screenPos);
        if(_plane.Raycast(_rayNow, out var enterNow))
        {
            return _rayNow.GetPoint(enterNow);
        }
            return Vector2.zero;
    }

    protected Vector3 PlanePositionDelta(Touch touch)
    {
        if(touch.phase != TouchPhase.Moved)
        return Vector3.zero;

        var _rayBefore = _cam.ScreenPointToRay(touch.position - touch.deltaPosition);
        var _rayNow = _cam.ScreenPointToRay(touch.position);
        if(_plane.Raycast(_rayBefore, out var enterBefore) && _plane.Raycast(_rayNow, out var enterNow))
        {
            return _rayBefore.GetPoint(enterBefore) - _rayNow.GetPoint(enterNow);
        }

        return Vector3.zero;
    }

#endif
}
   
