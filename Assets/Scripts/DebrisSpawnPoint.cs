using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DebrisSpawnPoint : MonoBehaviour
{

    [SerializeField] private List<float> spawnTime;
    [SerializeField] private GameObject debrid;
    [SerializeField] private float initDelay;

    void Start()
    {
        StartCoroutine(InitDelay());
    }


    private IEnumerator InitDelay()
    {
        yield return new WaitForSeconds(initDelay);

        yield return StartCoroutine(Loop());
    }

    private IEnumerator Loop()
    {
        while (true)
        {
            foreach (var t in spawnTime)
            {
                Instantiate(debrid, transform);
                yield return new WaitForSeconds(t);
            }
        }
        
    }
    
    
}
