using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsGenerator : MonoBehaviour
{
    public GameObject[] propsTemplates;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(propsTemplates[Random.Range(0,propsTemplates.Length)], transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
