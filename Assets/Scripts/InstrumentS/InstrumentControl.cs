using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class InstrumentControl : MonoBehaviour
{
    [SerializeField] private float waitToDestroy = 1f;
    protected void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("enter");
            StartCoroutine(nameof(TimerDestroyer));
        }
    }

    protected void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            
            Debug.Log("exit");
            StopCoroutine(nameof(TimerDestroyer));
        }
    }
    
    protected IEnumerator TimerDestroyer()
    {
        yield return new WaitForSeconds(waitToDestroy);
        Destroy(gameObject);
    }
}
