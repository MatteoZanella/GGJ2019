using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageBar : MonoBehaviour
{
    public float maxRage = 100f;
    [Min(0f)] public float startRage = 30f;
    private float currentRage;
    private bool freezed;
    [SerializeField] private Image Bar;
    
    // Start is called before the first frame update
    void Start()
    {
        currentRage = startRage;
    }

    // Update is called once per frame
    void Update()
    {
        if (freezed)
            return;
        currentRage += Time.deltaTime;
        Bar.fillAmount = currentRage / maxRage;
    }

    public IEnumerator Freeze(float freezeTime)
    {
        freezed = true;
        yield return new WaitForSeconds(freezeTime);
        freezed = false;
    }

    public void ChangeRage(float offset)
    {
        currentRage += offset;
    }

    public float getCurrentRage()
    {
        return currentRage;
    }
}
