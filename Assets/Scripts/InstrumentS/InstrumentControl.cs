using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum InstrumentType
{
    Red,
    Yellow,
    Green,
    Blue,
    White
}



public class InstrumentControl : MonoBehaviour
{
    [SerializeField] protected float waitToDestroy = 1f;
    [SerializeField] protected AudioSource soundSource;

    public InstrumentType type;
    private void Start()
    {
        soundSource = FindObjectOfType<AudioSource>();
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
            SoundManager.Instance.ReceiveSound(type);
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
        InstrumentManager.Instance.FindAndDeleteInstrument(gameObject);
        
    }
}
