using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitpoints : MonoBehaviour
{
    public float Max;
    public float Current;

    private HitSplat splat;

    // Start is called before the first frame update
    void Awake()
    {
        Current = Max;
        splat = GetComponent<HitSplat>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damage)
    {
        Current -= damage;
        Current = Mathf.Max(0, Current);
        if (splat != null)
        {
            splat.Splat();
        }
    }

    public void Reset()
    {
        Current = Max;
    }
}
