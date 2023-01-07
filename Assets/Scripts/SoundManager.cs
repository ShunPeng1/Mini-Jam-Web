using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using TurnTheGameOn.Timer;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static SoundManager Instance;
    
    [SerializeField] private Transform circleHeatMap ;
    
    [Header("Type Define")]
    [SerializeField] private List<float> ranges;
    [SerializeField] private List<GameObject> eggPrefabs;
    [SerializeField] private List<InstrumentNote> instrumentNotes;

    [Serializable]
    public class InstrumentNote
    {
        
        // Note that we cannot use 2 same InstrumentType at the same correctTime 
        public InstrumentType type;
        public float correctTime;
    }

    [Header("Time")]
    [SerializeField] private Timer timer;
    [SerializeField] private float totalTimePhase;
    [SerializeField] private float offsetTime;
    [SerializeField] private int indexSince2NdBeating;
    [SerializeField] private int currentIndexBeating;
    [SerializeField] private List<double> timeOfFirstBeat;
    
    void Start()
    {
        Instance = this;
        
        
        int countNumFirstBeat = 0;
        foreach (var t in instrumentNotes)
        {
            float degree = DegreeOfTime(t.correctTime);
            
            // 12h is 0 degree clockwise, 3h is 90 , 6h is 180 , 9h is 270; 
            int typeIndex = TypeIndex(t.type);
            float xPos = -ranges[typeIndex] * Mathf.Cos(degree* Mathf.Deg2Rad);
            float yPos = ranges[typeIndex] * Mathf.Sin(degree * Mathf.Deg2Rad);

            Instantiate(eggPrefabs[typeIndex], new Vector2(xPos, yPos), Quaternion.Euler(0,0,-(degree-90f)), circleHeatMap);

            if (t.correctTime == 0f)
            {
                countNumFirstBeat++;
            }
        }
        
        var sorted = instrumentNotes.OrderBy(note => note.correctTime).ThenBy(note => note.type);
        
        //init the time of first beat
        timeOfFirstBeat = new List<float>(countNumFirstBeat);
        for (int i = 0; i < countNumFirstBeat; i++)
        {
            timeOfFirstBeat[i] = -1;
        }

        indexSince2NdBeating = countNumFirstBeat;

    }

    private float DegreeOfTime(float currentTime)
    {
        
        float degree = 90f +  360 * ( currentTime / totalTimePhase);
    }

    private int TypeIndex(InstrumentType type)
    {
        switch (type)
        {
            case InstrumentType.Red:
                return 0;
            
            case InstrumentType.Yellow:
                return 1;
            
            case InstrumentType.Green:
                return 2;
            
            case InstrumentType.Blue:
                return 3;
            
            case InstrumentType.White:
                return 4;
        }

        return -1;
    }

    private void ResetBeat()
    {
        
    }
    
    public void ReceiveSound(InstrumentType type)
    {
        if (indexSince2NdBeating != 0)
        {
            
        }
        else
        {
            if (timeOfFirstBeat[ TypeIndex(type) ] == -1f)
            {
                timeOfFirstBeat[TypeIndex(type)] = timer.GetTimerValue();
            }
            else
            {
                ResetBeat();
            }
        }
    }
    
}
