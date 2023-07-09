using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Music : MonoBehaviour
{
    private StudioEventEmitter musicEmitter;

    private void Start()
    {
        musicEmitter = GetComponent<StudioEventEmitter>();
    }

    void Update()
    {
        musicEmitter.SetParameter("Intensity", 1 - GameManager.Instance.GetLowestTraitValue());
    }
}
