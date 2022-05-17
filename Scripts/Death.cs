using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private Rigidbody _rigid;

    [SerializeField] private Transform reset;
    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_rigid.velocity.y < -100f)
        {
            transform.position = reset.position;
            _rigid.velocity = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.R))
        {
            transform.position = reset.position;
            _rigid.velocity = Vector3.zero;
        }
    }
}
