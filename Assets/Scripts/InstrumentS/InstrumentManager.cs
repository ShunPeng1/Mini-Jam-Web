using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InstrumentManager : MonoBehaviour
{
    public static InstrumentManager Instance; 
    void Start()
    {
        Instance = this;
        allCurrentInstruments = new List<GameObject>();
    }

    [SerializeField] private List<GameObject> allCurrentInstruments;
    [SerializeField] private List<GameObject> instrumentsSpawn;
    [SerializeField] private float instrumentsWidth = 0.1f;


    [Header("Range of instrument")] 
    [SerializeField] private float range0;
    [SerializeField] private float range1;
    [SerializeField] private float range2;
    [SerializeField] private float range3;

    [Header("Sound Source")] public AudioSource soundSource;
    
    private int RangeIndex(float sizeMagnitude)
    {
        if (sizeMagnitude < range0)
        {
            return -1;
        }

        if (sizeMagnitude < range1)
        {
            return 0;
        }
        if (sizeMagnitude < range2)
        {
            return 1;
        }
        if (sizeMagnitude < range3)
        {
            return 2;
        }

        return 3;
    }
    
    public void PlaceInstrument(Vector3 firstPosition, Vector3 secondPosition)
    {
        Vector3 middlePosition = (firstPosition + secondPosition) / 2;
        Vector3 deltaPosition = (secondPosition - firstPosition);
        if(deltaPosition.magnitude == 0) return;
        Vector3 size =  new Vector3( deltaPosition.magnitude  ,instrumentsWidth * (deltaPosition.y >=0 ? 1: -1),0) ;
        
        Vector3 rotation = new Vector3(0,0, Mathf.Acos( deltaPosition.normalized.x * (deltaPosition.y >=0? 1:-1 ) / deltaPosition.normalized.magnitude  )* Mathf.Rad2Deg);

        int index = RangeIndex(deltaPosition.magnitude);
        if (index < 0)
        {
            return;
        }
        
        GameObject instantiated = Instantiate(instrumentsSpawn[index] ,middlePosition, Quaternion.Euler(rotation));
        instantiated.transform.localScale = size;
        
        allCurrentInstruments.Add(instantiated);
    }

    
}

