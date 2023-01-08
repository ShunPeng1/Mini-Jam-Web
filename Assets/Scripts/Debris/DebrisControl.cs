using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisControl : MonoBehaviour
{
    [SerializeField] private float existingTime = 30f;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(existingTime);
        Destroy(gameObject);
    }
}
