using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumControl : InstrumentControl
{
    [SerializeField]private AudioClip heavyTomClip;
    [SerializeField]private AudioClip lightTomClip;
    [SerializeField]private AudioClip edgeTomClip;

    [SerializeField] private float rangePercentage0;
    [SerializeField] private float rangePercentage1;
    
    
    protected override void PlaySound(Transform colPosition)
    {

        float objRange = (colPosition.position - transform.position).magnitude;
        float currentRange = (transform.lossyScale.magnitude) * 0.5f;

        Debug.Log(objRange.ToString() + " and " + currentRange.ToString());
        if ( objRange < rangePercentage0 * currentRange)
        {
            soundSource.PlayOneShot(heavyTomClip);
        }

        if ( objRange < rangePercentage1 * currentRange)
        {
            soundSource.PlayOneShot(lightTomClip);
        }
        
        soundSource.PlayOneShot(edgeTomClip);
        
    }
    
    
}
