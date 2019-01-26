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
        if (transform.parent != null)
            return;
        var tg = GameObject.FindWithTag("GameController").transform;
        CameraTarget.LookAt(tg);
        if (Input.GetKeyDown(KeyCode.Space) && timeOut <= 0)
        {
            timeOut += 0.5f;
            Vector3 dir = tg.position - p1.position;
            p1.AddForce(dir.normalized * power + Vector3.up*power/2f);
            p1.AddRelativeTorque(rotation);
            p2.AddRelativeTorque(-rotation);
        }

        timeOut -= Time.deltaTime;
    }
}