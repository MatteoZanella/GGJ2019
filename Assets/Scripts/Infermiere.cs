using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class Infermiere : MonoBehaviour
{
    private NavMeshAgent _agent;

    private Animator _animator;

    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
            return;
        _agent.SetDestination(GameObject.FindWithTag("Player").transform.position);
        _animator.SetFloat(Speed, _agent.desiredVelocity.magnitude);
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.impulse.magnitude > 10 && GameObject.FindWithTag("GameController").GetComponent<WheelChair>()._boostOn)
        {
            _animator.SetTrigger(Hit);
            _agent.isStopped = true;
            canMove = false;
            GetComponent<CapsuleCollider>().enabled = false;
            _agent.enabled = false;
        }
    }
}
