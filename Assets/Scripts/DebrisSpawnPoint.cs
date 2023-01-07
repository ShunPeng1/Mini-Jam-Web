using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DebrisSpawnPoint : MonoBehaviour
{

    [SerializeField] private List<float> spawnTime;
    [SerializeField] private List< GameObject > debris;
    [SerializeField] private float initDelay;

    private IEnumerator OnEnable()
    {
        yield return new WaitForSeconds(initDelay);

        yield return StartCoroutine(Loop());
        
    }



    private IEnumerator Loop()
    {
        while (true)
        {
            foreach (var time in spawnTime)
            {
                int index = Random.Range(0, debris.Count);
                Instantiate(debris[ index ], transform);
                yield return new WaitForSeconds(time);
            }
        }
        
    }
    
    
}
