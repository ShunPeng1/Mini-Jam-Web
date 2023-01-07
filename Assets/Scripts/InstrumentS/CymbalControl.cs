using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CymbalControl : InstrumentControl
{
    [SerializeField]private AudioClip cymbalClip;
    protected override void PlaySound(Transform colPosition)
    {
        soundSource.PlayOneShot(cymbalClip);
    }
}

