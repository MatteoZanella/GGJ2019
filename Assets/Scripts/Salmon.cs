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

        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f && timeOut <= 0)
        {
            timeOut += 0.4f;
            p1.AddForce((Camera.main.transform.right * Input.GetAxis("Horizontal")) * power + Vector3.up * power);
            Shake();
        }
        else if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f && timeOut <= 0)
        {
            timeOut += 0.5f;
            Vector3 dir = Camera.main.transform.forward;
            dir.y = 0;
            p1.AddForce(dir.normalized * Input.GetAxis("Vertical") * power + Vector3.up * power);
            Shake();
        }
        else if (timeOut <= 0)
        {
            timeOut += 0.8f;
            Vector3 dir = tg.position - p1.position;
            p1.AddForce(dir.normalized * power + Vector3.up * power);
            Shake();
        }
        timeOut -= Time.deltaTime;
    }

    void Shake()
    {
        p1.AddRelativeTorque(rotation*Random.Range(-1f,1f));
        p2.AddRelativeTorque(rotation*Random.Range(-1f,1f));
    }
}