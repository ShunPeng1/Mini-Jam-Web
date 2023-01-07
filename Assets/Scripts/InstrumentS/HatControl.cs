using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatControl : InstrumentControl
{
    
    
    [SerializeField]private AudioClip closeHatClip;
    [SerializeField]private AudioClip hiHatClip;

    [SerializeField] private float rangePercentage0;
    
    
    protected override void PlaySound(Transform colPosition)
    {

        float objRange = (colPosition.position - transform.position).magnitude;
        float currentRange = (transform.lossyScale.magnitude) * 0.5f;
        
        if ( objRange < rangePercentage0 * currentRange)
        {
            soundSource.PlayOneShot(closeHatClip);
        }

        
        soundSource.PlayOneShot(hiHatClip);
        
    }
}
