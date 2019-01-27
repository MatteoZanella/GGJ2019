using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
        
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("GameController"))
        {
            RageBar.instance.ChangeRage(-10);
            Destroy(gameObject);
        }
    }
}
