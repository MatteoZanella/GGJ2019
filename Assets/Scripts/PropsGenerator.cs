using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PropsGenerator : MonoBehaviour
{
    public GameObject[] propsTemplates;

    public GameObject memory;
    public GameObject infermiere;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject propObject = Instantiate(propsTemplates[Random.Range(0, propsTemplates.Length)], transform);
        for (int i =0; i<6; i++)
        {
            if (Random.Range(0, 100) < 30)
                continue;
            GameObject newMemory = Instantiate(memory, transform);
            newMemory.transform.localPosition = new Vector3(Random.Range(-4f,4f),0.4f,Random.Range(-4f,4f));
        }
        for (int i =0; i<3; i++)
        {
            if (Random.Range(0, 100) < 30)
                continue;
            GameObject newMemory = Instantiate(infermiere, transform);
            newMemory.transform.localPosition = new Vector3(Random.Range(-9f,9f),1f,Random.Range(-9f,9f));
            newMemory.transform.localScale = Vector3.one/2f;
        }
    }
}
