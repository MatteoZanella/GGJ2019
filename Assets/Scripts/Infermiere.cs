using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Infermiere : MonoBehaviour
{
    private NavMeshAgent _agent;

    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(GameObject.FindWithTag("Player").transform.position);
        _animator.SetFloat("Speed", _agent.desiredVelocity.magnitude);
    }
}
