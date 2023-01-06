using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisRemover : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Debris"))
        {
            Destroy(other.gameObject);
        }
    }
}
