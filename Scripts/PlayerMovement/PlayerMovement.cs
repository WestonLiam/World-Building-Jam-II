using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rBody;
    private Camera _mainCamera;

    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField][Range(0,1)] private float _airControl;
    //[SerializeField] private float _acceleration;
    //[SerializeField] private float _deceleration;

    private bool _jumpNextFixedUpdate;
    private bool _jumpKeyDown;
    [SerializeField] private float _jumpHeight;

    [SerializeField] private float _fallGravity;
    [SerializeField] private float _jumpGravity;
    [SerializeField] private float _jumpToFallGravityDelta;
    private float _currentJumpGravity;

    [field: SerializeField] public bool OnGround{ get; private set; }

    public KeyCode JumpKey { get; set; } = KeyCode.Space;
    public KeyCode RunKey { get; set; } = KeyCode.LeftShift;

    // Start is called before the first frame update
    void Start()
    {
        _rBody = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
    }

    void Update()
    {
        OnGround = CheckGround();

        if (_jumpKeyDown)
        {
            _jumpNextFixedUpdate = true;
        }

        if (Input.GetKey(JumpKey))
        {
            _jumpKeyDown = true;
        }

        else
        {
            _jumpKeyDown = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        ApplyCustomGravity();
        RotateWithCamera();

        if (_jumpNextFixedUpdate)
        {
            if (OnGround)
            {
                Jump();
            }

            _jumpNextFixedUpdate = false;
        }

        Walk();
    }

    private bool CheckGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, transform.localScale.y/2f + 1f))
        {
            return true;
        }

        return false;
    }

    private void Walk()
    {
        Vector3 velocity = _rBody.velocity;
        Vector3 targetVelocity;

        float speed = Input.GetKey(RunKey) ? _runSpeed : _walkSpeed;

        if (OnGround)
        {
            Vector3 targetVelocityX = _mainCamera.transform.right * Input.GetAxis("Horizontal");
            Vector3 targetVelocityY = _mainCamera.transform.forward * Input.GetAxis("Vertical");

            targetVelocity = (targetVelocityX + targetVelocityY) * speed;
        }
        else
        {
            Vector3 targetVelocityX = _mainCamera.transform.right * Input.GetAxis("Horizontal");
            Vector3 targetVelocityY = _mainCamera.transform.forward * Input.GetAxis("Vertical");

            targetVelocity = Vector3.Slerp(velocity, (targetVelocityX + targetVelocityY) * _walkSpeed,  _airControl);
        }

        /* Acceleration code
        float maxAccelerationX = (targetVelocity.x > velocity.x ? _acceleration : _deceleration) * Time.deltaTime;
        float maxAccelerationZ = (targetVelocity.z > velocity.z ? _acceleration : _deceleration) * Time.deltaTime;

        velocity.x = Mathf.MoveTowards(velocity.x, targetVelocity.x, maxAccelerationX);
        velocity.z = Mathf.MoveTowards(velocity.z, targetVelocity.z, maxAccelerationZ);
        */

        _rBody.velocity = new Vector3(targetVelocity.x, velocity.y, targetVelocity.z);
    }

    private void RotateWithCamera()
    {
        Vector3 targetRotation = new Vector3 (transform.rotation.x, _mainCamera.transform.rotation.eulerAngles.y, transform.rotation.z);

        transform.rotation = Quaternion.Euler(targetRotation);
    }

    private void Jump() 
    {
        _currentJumpGravity = _jumpGravity;
        _rBody.velocity += new Vector3(_rBody.velocity.x, _jumpHeight, _rBody.velocity.z);
    }

    private void ApplyCustomGravity()
    {
        if (_rBody.velocity.y > 0 && _jumpKeyDown)
        {
            _currentJumpGravity = Mathf.MoveTowards(_currentJumpGravity, _fallGravity, _jumpToFallGravityDelta * Time.deltaTime);

            _rBody.velocity += Vector3.up * Physics.gravity.y * _currentJumpGravity * Time.deltaTime;
        }

        else if (!OnGround)
        {
            _rBody.velocity += Vector3.up * Physics.gravity.y * _fallGravity * Time.deltaTime;
        }

    }
}
