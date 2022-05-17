using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [SerializeField] private float _rotSpeed;
    [SerializeField] private Vector2 _rotClamp;

    [SerializeField] private float _focusRadius;
    [SerializeField] [Range(0,1)] private float _focusCenteringSensitivity;

    [SerializeField] private float _offset;

    private Vector2 _orbitAngles;
    private Vector3 _focusPoint;

    // Start is called before the first frame update
    void Start()
    {
        if (_target == null)
        {
            enabled = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
        _focusPoint = _target.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        
    }

    void FixedUpdate()
    {
        UpdateCamera();
        UpdateOrbitAngles();
    }

    private void UpdateCamera()
    {
        float distance = Vector3.Distance(_target.position, _focusPoint) + _offset;

        _focusPoint = UpdateFocusPoint(distance);
        //_focusPoint = Vector3.Lerp(_target.position, transform.position, .5f); ;

        Quaternion lookRotation = Quaternion.Euler(_orbitAngles);
        Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = _focusPoint - lookDirection * distance;

        transform.SetPositionAndRotation(lookPosition, lookRotation);
    }
    
    private Vector3 UpdateFocusPoint(float distance) 
    {
        float interpSpeed = 1f;
    
        if (_focusRadius > 0.01f && _focusCenteringSensitivity > 0f)
        {
            interpSpeed = Mathf.Pow(1f - _focusCenteringSensitivity, Time.unscaledDeltaTime);
        }
    
        if (distance > _focusRadius)
        {
            interpSpeed = Mathf.Min(interpSpeed, _focusRadius / distance);
        }

        Vector3 focusPoint = Vector3.Lerp(_target.position, _focusPoint, interpSpeed);
    
        return focusPoint;
    }

    private void UpdateOrbitAngles()
    {
        Vector2 input = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

        float e = .001f;

        if (input.x < -e || input.x > e || input.y < -e || input.y > e)
        {
            _orbitAngles += _rotSpeed * input;
            _orbitAngles = new Vector2(Mathf.Clamp(_orbitAngles.x, _rotClamp.x, _rotClamp.y), _orbitAngles.y);
        }
    }
}
