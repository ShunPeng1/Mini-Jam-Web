using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class InstrumentControl : MonoBehaviour
{
    [SerializeField] protected float waitToDestroy = 1f;
    [SerializeField] protected AudioSource soundSource;
    
    private void Start()
    {
        soundSource = SoundManager.Instance.GetAudioSource();
    }

    protected void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            StartCoroutine(nameof(TimerDestroyer));
        }

        if (col.gameObject.CompareTag("Debris"))
        {
            PlaySound(col.transform);
        }
    }

    protected virtual void PlaySound(Transform colPosition)
    {
        
    }
    
    protected void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            StopCoroutine(nameof(TimerDestroyer));
        }
    }
    
    protected IEnumerator TimerDestroyer()
    {
        yield return new WaitForSeconds(waitToDestroy);
        Destroy(gameObject);
    }
}
