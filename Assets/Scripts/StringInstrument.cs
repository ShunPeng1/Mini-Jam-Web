using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class StringInstrument : MonoBehaviour
{
    [SerializeField] private float waitToDestroy = 1f;
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("enter");
            StartCoroutine(nameof(TimerDestroyer));
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            
            Debug.Log("exit");
            StopCoroutine(nameof(TimerDestroyer));
        }
    }
    
    private IEnumerator TimerDestroyer()
    {
        yield return new WaitForSeconds(waitToDestroy);
        Destroy(gameObject);
    }
}
