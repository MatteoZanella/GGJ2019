using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    public Animator animator;

    private static readonly int Destroy1 = Animator.StringToHash("Destroy");
    // Start is called before the first frame update
        
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("GameController"))
        {
            RageBar.instance.ChangeRage(-10);
            animator.SetTrigger(Destroy1);
            enabled = false;
            Destroy(gameObject, 1f);
        }
    }
}
