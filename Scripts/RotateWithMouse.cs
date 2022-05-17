using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateWithMouse : MonoBehaviour
{
    [SerializeField] private float _rotSpeed;
    //[SerializeField] private Transform _rotateTarget;

    private Vector2 _orbitAngles;

    void LateUpdate()
    {
        UpdateOrbitAngles();
    }

    void FixedUpdate()
    {
        Rotate();
    }

    private void UpdateOrbitAngles()
    {
        Vector2 input = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

        float e = .001f;

        if (input.x < -e || input.x > e || input.y < -e || input.y > e)
        {
            _orbitAngles += _rotSpeed * input;
            _orbitAngles = new Vector2(0, _orbitAngles.y);
        }
    }

    private void Rotate()
    {
        Quaternion lookRotation = Quaternion.Euler(_orbitAngles);

        transform.rotation = lookRotation;
    }

    private void RotateAroundTarget()
    {

    }
}
