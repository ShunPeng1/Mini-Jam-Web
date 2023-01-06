using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DebrisSpawnPoint : MonoBehaviour
{

    [SerializeField] private List<float> spawnTime;
    [SerializeField] private GameObject debris;
    [SerializeField] private float initDelay;

    private IEnumerator Start()
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
                Instantiate(debris, transform);
                yield return new WaitForSeconds(time);
            }
        }
        
    }
    
    
}
