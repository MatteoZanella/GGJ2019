using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class WheelChair : MonoBehaviour
{
    public float speed;
    public float steer;
    [SerializeField] private Rigidbody _rigidbody;
    public Vector3 fallOffset = new Vector3(0,0,70f);
    [Header("Boost")] public float duration = 2.5f;
    public float speedMultiplier = 2.5f;
    [SerializeField] private ParticleSystem boostParticles;
    public bool _boostOn = false, _boosted = false;

    [Header("Eject")] public FixedJoint[] joints;
    public Vector3 ejectPower;
    public Transform character;
    public LayerMask defaultLayer;
    private bool _ejected;

    [Header("character")] public GameObject prefab;
    private float _ejectTime = 0, _boostTime;
    public Transform com;

    private void Start()
    {
        StartCoroutine(Dab());
        WorldGenerator.instance.player = transform;
    }

    IEnumerator Dab()
    {
        character.GetComponent<Salmon>().CameraTarget.eulerAngles = transform.TransformDirection(Vector3.back);
        Camera.main.GetComponent<SmoothFollow>().target = character.GetComponent<Salmon>().CameraTarget;
        yield return new WaitForSeconds(0.5f);
        Camera.main.GetComponent<SmoothFollow>().target = transform;
    }
    
    
    void Update()
    {
        _rigidbody.centerOfMass = com.localPosition;
        _ejectTime += Time.deltaTime;
        if (_ejected && !_boostOn)
            return;
        float fast = speed;
        if (_boostOn)
        {
            _boostTime += Time.deltaTime;
            fast *= Mathf.Lerp(transform.InverseTransformDirection(_rigidbody.velocity).z / speed, speedMultiplier, _boostTime / (duration+0.6f));
        } else if (_boosted) {
            _boostTime += Time.deltaTime;
            fast *= Mathf.Lerp(speedMultiplier, Input.GetAxis("Vertical"), _boostTime / (duration/2));
        } else
        {
            fast *= Input.GetAxis("Vertical");
        }
        float str = Input.GetAxis("Horizontal") * steer;
        transform.Rotate(Vector3.up, str);
        var localVel = transform.InverseTransformDirection(_rigidbody.velocity);
        localVel.z = fast;
        _rigidbody.velocity = transform.TransformDirection(localVel);
        if (_ejected)
            return;
        if (transform.localEulerAngles.x > fallOffset.x && transform.localEulerAngles.x < 270 ||
            transform.localEulerAngles.x > 270 && transform.localEulerAngles.x < 360-fallOffset.x||
            transform.localEulerAngles.z > fallOffset.z && transform.localEulerAngles.z < 90  ||
            transform.localEulerAngles.z > 270 && transform.localEulerAngles.z < 360-fallOffset.z)
            Eject(false);
        if (Input.GetKeyDown(KeyCode.E))
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
        _boostTime = 0;
        yield return new WaitForSeconds(duration);        
        boostParticles.Stop();
        _boostOn = false;

        _boostTime = 0;
        _boosted = true;
        yield return new WaitForSeconds(duration/2);
        _boosted = false;
    }


    void Eject(bool launch = true)
    {
        _ejected = true;
        foreach (FixedJoint joint in joints)
        {
            Rigidbody rb = joint.connectedBody;
            joint.connectedBody = null;
            if (rb && launch)
                rb.AddRelativeForce(ejectPower);
        }

        RecSetLayer(character);
        character.SetParent(null);
        WorldGenerator.instance.player = character.GetComponent<Salmon>().CameraTarget;
        Camera.main.GetComponent<SmoothFollow>().target = character.GetComponent<Salmon>().CameraTarget;
        Destroy(GetComponent<FixedJoint>());
        _ejectTime = 0;
    }

    void RecSetLayer(Transform tr)
    {
        tr.gameObject.layer = defaultLayer;
        foreach (Transform t in tr)
            RecSetLayer(t);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_ejected || _ejectTime < 1f || !other.CompareTag("Player"))
            return;
        _ejected = false;
        character.SetParent(transform);
        Destroy(gameObject);

        GameObject go = Instantiate(prefab, transform.position,
            Quaternion.EulerRotation(0, transform.eulerAngles.y, 0));
        go.GetComponent<WheelChair>().prefab = prefab;
    }
}