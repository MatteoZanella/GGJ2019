using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class WheelChair : MonoBehaviour
{
    public float speed;
    public float steer;
    [SerializeField]
    private Rigidbody _rigidbody;

    [Header("Boost")] public float duration = 2.5f;
    public float speedMultiplyer = 4;
    [SerializeField]
    private ParticleSystem boostParticles;
    private bool _boostOn = false;
    
    [Header("Eject")]
    public FixedJoint[] joints;
    public Vector3 ejectPower;
    public Transform character;
    public LayerMask defaultLayer;
    private bool _ejected;
   
    void Update()
    {
        if (_ejected)
            return;
        float fast = speed * (_boostOn ? speedMultiplyer : Input.GetAxis("Vertical"));
        float str = Input.GetAxis("Horizontal") * steer;
        transform.Rotate(Vector3.up, str);
        var localVel = transform.InverseTransformDirection(_rigidbody.velocity);
        localVel.z = fast;
        _rigidbody.velocity = transform.TransformDirection(localVel);
        if (transform.localEulerAngles.x > 60 && transform.localEulerAngles.x < 270)
            Eject();
        if (Input.GetKeyDown(KeyCode.Space))
            Eject();
        if (Input.GetKeyDown(KeyCode.B))
            Boost();
    }

    void Boost()
    {
        if (!_boostOn)
            StartCoroutine(TurnOnBoost());
    }

    IEnumerator TurnOnBoost()
    {
        _boostOn = true;
        boostParticles.Play();
        yield return new WaitForSeconds(duration);
        boostParticles.Stop();
        _boostOn = false;
    }
    

    void Eject()
    {
        _ejected = true;
        foreach (FixedJoint joint in joints)
        {
            Rigidbody rb = joint.connectedBody;
            joint.connectedBody = null;
            if (rb)
                rb.AddRelativeForce(ejectPower);
        }
        RecSetLayer(character);
        character.SetParent(null);
        Destroy(GetComponent<FixedJoint>());
    }

    void RecSetLayer(Transform tr)
    {
        tr.gameObject.layer = defaultLayer;
        foreach (Transform t in tr)
            RecSetLayer(t);
    }
}
