using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelChair : MonoBehaviour
{
    public float speed;
    public float steer;
    [SerializeField]
    private Rigidbody _rigidbody;
    void Update()
    {
        float fast = Input.GetAxis("Vertical") * speed;
        float str = Input.GetAxis("Horizontal") * steer;
        transform.Rotate(Vector3.up, str);
        var localVel = transform.InverseTransformDirection(_rigidbody.velocity);
        localVel.z = fast;
        _rigidbody.velocity = transform.TransformDirection(localVel);
    }
}
