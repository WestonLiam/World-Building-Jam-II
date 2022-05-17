using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorUpdater : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rigid;
    private PlayerMovement _movement;

    private bool _movingLastFrame;
    private bool _onGroundLastFrame;
    private float _framesSinceOnGround;
    private float _yRotLastFrame;

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _movement = GetComponent<PlayerMovement>();

        _yRotLastFrame = transform.rotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        AnimateMovement();
        //AnimateRotation();
    }

    private void AnimateMovement()
    {
        bool moving = _rigid.velocity.magnitude >= 1f;

        if (!_movement.OnGround)
        {
            _framesSinceOnGround++;
        }

        else
        {
            _framesSinceOnGround = 0;
            _animator.SetBool("OnGround", true);
        }

        if (_framesSinceOnGround >= 10)
        {
            _animator.SetBool("OnGround", false);
        }

        if (moving != _movingLastFrame)
        {
            if (Input.GetKey(_movement.RunKey))
            {
                Debug.Log("Running");
                _animator.SetBool("Running", moving);

                _movingLastFrame = moving;
            }

            else
            {
                _animator.SetBool("Walking", moving);

                _movingLastFrame = moving;
            }
        }
    }

    //private void AnimateRotation()
    //{
    //    _animator.SetFloat("RotSpeed", Mathf.RoundToInt(transform.rotation.y - _yRotLastFrame));
    //
    //    _yRotLastFrame = transform.rotation.y;
    //}
}
