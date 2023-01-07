using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TurnTheGameOn.Timer;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static SoundManager Instance;
    
    [SerializeField] private Transform circleHeatMap ;
    
    [Header("Radius")]
    [SerializeField] private List<float> ranges;
    [SerializeField] private float mainLength;

    
    [Serializable]
    public class InstrumentNote
    {
        public InstrumentType type;
        public GameObject prefabs;
        public int rangeIndex;
        public float correctTime;
        public float offsetTime;
    }

    [SerializeField] private Timer timer;

    [SerializeField] private List<InstrumentNote> instrumentNotes;

    
    void Start()
    {
        foreach (var t in instrumentNotes)
        {
            float degree = 90f +  360 * ( t.correctTime / mainLength);
            
            // 12h is 0 degree clockwise, 3h is 90 , 6h is 180 , 9h is 270; 

            float xPos = -ranges[t.rangeIndex] * Mathf.Cos(degree* Mathf.Deg2Rad);
            float yPos = ranges[t.rangeIndex] * Mathf.Sin(degree * Mathf.Deg2Rad);

            Instantiate(t.prefabs, new Vector2(xPos, yPos), Quaternion.identity, circleHeatMap);
            
            Debug.Log(degree.ToString() + " "+ xPos.ToString() + " " + yPos.ToString());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Play();
        }
    }
    
    private void Play()
    {
        
    }
}
