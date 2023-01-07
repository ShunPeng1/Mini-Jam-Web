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
        
        foreach (var t in instrumentNotes)
        {
            float degree =  90f +  360f * ( t.correctTime / totalTimePhase);
            
            // 12h is 0 degree clockwise, 3h is 90 , 6h is 180 , 9h is 270; 
            int typeIndex = TypeIndex(t.type);
            float xPos = -ranges[typeIndex] * Mathf.Cos(degree* Mathf.Deg2Rad);
            float yPos = ranges[typeIndex] * Mathf.Sin(degree * Mathf.Deg2Rad);
            Debug.Log(typeIndex+" "+ degree+" "+ xPos.ToString() + " " + yPos.ToString());
            Instantiate(eggPrefabs[typeIndex], new Vector2(xPos, yPos), Quaternion.Euler(0,0,-(degree-90f)), circleHeatMap);

            
        }
        
        var sorted = instrumentNotes.OrderBy(note => note.correctTime).ThenBy(note => note.type);
        
    }

    private float DegreeOfTime(float currentTime)
    {
        return 90f +  360f * ( currentTime / totalTimePhase);
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
        timer.SetTimerValue(0);
        timer.timerState = TimerState.Disabled;
    }

    private List<bool> _isBeatNotes;
    private bool _isBeating = false;
    private void StartFirstBeat()
    {
        timer.RestartTimer();
        timer.finishTime = totalTimePhase;
    }

    private void CheckWinning()
    {
        
    }
    
    public void ReceiveSound(InstrumentType type)
    {
        Debug.Log(Time.time);
        
        if (_isBeating)
        {
            for (int i = 0 ; i< instrumentNotes.Count; i++)
            {
                var t = instrumentNotes[i];

                if (Mathf.Abs( (float)(t.correctTime - timer.GetTimerValue()) ) <=  offsetTime)
                {
                    if (_isBeatNotes[i])
                    { 
                        ResetBeat();
                        _isBeating = false;
                    }
                    else
                    {
                        _isBeatNotes[i] = true;
                    }
                }
                
            }
        }
        else
        {
            for (int i = 0 ; i< instrumentNotes.Count; i++)
            {
                var t = instrumentNotes[i];
                if (t.correctTime > 0)
                {
                    return;
                }
                if (t.type == type)
                {
                    StartFirstBeat();
                    _isBeating = true;
                    _isBeatNotes = new List<bool>(instrumentNotes.Count+1);
                    _isBeatNotes[i] = true;
                }
            }
        }
        
    }
    

}
