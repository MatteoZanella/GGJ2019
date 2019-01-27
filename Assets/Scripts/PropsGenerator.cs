using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PropsGenerator : MonoBehaviour
{
    public GameObject[] propsTemplates;

    public GameObject memory;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject propObject = Instantiate(propsTemplates[Random.Range(0, propsTemplates.Length)], transform);
        while (Random.Range(0, 100) < 35)
        {
            GameObject newMemory = Instantiate(memory, transform);
            newMemory.transform.localPosition = new Vector3(Random.Range(-4f,4f),0.4f,Random.Range(-4f,4f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
