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
    }

    [SerializeField] private List<GameObject> allInstruments;
    [SerializeField] private List<GameObject> realInstruments;
    [SerializeField] private List<GameObject> ghostInstruments;

    private int _lastObjectIndex = -1;    


    [Header("Range of instrument")] 
    [SerializeField] private float range0;
    [SerializeField] private float range1;
    [SerializeField] private float range2;
    [SerializeField] private float range3;
    [SerializeField] private float range4;

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
        if (sizeMagnitude < range4)
        {
            return 3;
        }
        return -1;
    }
    
    public void PlaceInstrument(Vector3 firstPosition, Vector3 secondPosition, bool isGhost = false)
    {
        Vector3 middlePosition = (firstPosition + secondPosition) / 2;
        Vector3 deltaPosition = (secondPosition - firstPosition);
        if(deltaPosition.magnitude == 0) return;
        Vector3 size =  new Vector3( deltaPosition.magnitude  , (deltaPosition.y >=0 ? 1: -1),0) ;
        
        Vector3 rotation = new Vector3(0,0, Mathf.Acos( deltaPosition.normalized.x * (deltaPosition.y >=0? 1:-1 ) / deltaPosition.normalized.magnitude  )* Mathf.Rad2Deg);

        int index = RangeIndex(deltaPosition.magnitude);
        if (index < 0)
        {
            return;
        }
        
        GameObject instantiated = Instantiate((isGhost? ghostInstruments[index]: realInstruments[index]) ,middlePosition, Quaternion.Euler(rotation));
        instantiated.transform.localScale = size;
        
        allInstruments.Add(instantiated);
        _lastObjectIndex = isGhost? index: -1;
    }

    public void DestroyLastInserted()
    {
        int index = allInstruments.Count - 1;
        if (index < 0 ) return;   
        GameObject lastInsert =  allInstruments[index];
        if(lastInsert != null) Destroy(lastInsert);
        allInstruments.RemoveAt(index);
        _lastObjectIndex = -1;
    }

    public void DestroyAll()
    {
        foreach (var instrument in allInstruments)
        {
            if(instrument != null) Destroy(instrument);
        }

        allInstruments = new List<GameObject>();
    }

    public void ReTransformOrInstantiateInLastInsert(Vector3 firstPosition, Vector3 secondPosition)
    {
        // resize the new object from pplayer movement, if it is not in range of last object, instaintiate it instead
        Vector3 deltaPosition = (secondPosition - firstPosition);
        
        int index = RangeIndex(deltaPosition.magnitude);
        
        
        
        if (_lastObjectIndex != index)
        {
            if(_lastObjectIndex != -1) DestroyLastInserted();
            PlaceInstrument(firstPosition,secondPosition, true);
            return;
        }

        if (index < 0 ) return;

        _lastObjectIndex = index;
        Vector3 middlePosition = (firstPosition + secondPosition) / 2;
        if(deltaPosition.magnitude == 0) return;
        Vector3 size =  new Vector3( deltaPosition.magnitude  , (deltaPosition.y >=0 ? 1: -1),0) ;
        
        Vector3 rotation = new Vector3(0,0, Mathf.Acos( deltaPosition.normalized.x * (deltaPosition.y >=0? 1:-1 ) / deltaPosition.normalized.magnitude  )* Mathf.Rad2Deg);
        
        int lastInsertedIndex = allInstruments.Count - 1;
        Transform lastInsert =  allInstruments[lastInsertedIndex].transform;
        lastInsert.position = middlePosition;
        lastInsert.rotation = Quaternion.Euler( rotation);
        lastInsert.localScale = size;
    }
    
}

