using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salmon : MonoBehaviour
{
    public Vector3 rotation;
    public float power;
    public Rigidbody p1, p2;
    public Transform CameraTarget;

    private float timeOut = 2;

    void Update()
    {
        if (transform.parent == null && Input.GetKeyDown(KeyCode.Space) && timeOut <= 0)
        {
            timeOut += 0.5f;
            Vector3 dir = GameObject.FindWithTag("GameController").transform.position - p1.position;
            p1.AddForce(dir.normalized * power + Vector3.up*power/2f);
                /*Camera.main.transform.TransformDirection(Vector3.up * power +
                                                                 Vector3.forward * Input.GetAxis("Vertical") * power +
                                                                 Vector3.right * Input.GetAxis("Horizontal") * power));
            */
            p1.AddRelativeTorque(rotation);
            p2.AddRelativeTorque(-rotation);
        }

        timeOut -= Time.deltaTime;
    }
}