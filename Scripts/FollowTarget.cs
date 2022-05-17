using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField][Range(0,1)] private float _interpSpeed;
    
    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 currentPos = transform.position;
        Vector3 newPos = Vector3.Slerp(currentPos, _target.position + _offset, _interpSpeed);

        transform.position = newPos;
    }
}
