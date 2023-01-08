using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using TurnTheGameOn.Timer;
using Unity.VisualScripting;
using Timer = TurnTheGameOn.Timer.Timer;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static SoundManager Instance;

    [Header("Transform of object")]
    [SerializeField] private GameObject clockHandTransform;
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

        _isBeating = false;
    }

    private List<bool> _isBeatNotes;
    private bool _isBeating = false;
    private float _nextBeatTimeCheck;
    private int _loopCounter = 0;
    
    private void StartFirstBeat()
    {
        timer.RestartTimer();
        timer.finishTime = totalTimePhase;
        
        _isBeating = true;
        _nextBeatTimeCheck = offsetTime;
        
        _isBeatNotes = new List<bool>();

        for (int j = 0; j < instrumentNotes.Count; j++)
        {
            _isBeatNotes.Add(false);
        }

    }

    private bool  CheckWinning()
    {
        foreach (var t in _isBeatNotes)
        {
            if (t == false) return false;
        }

        return true;
    }

    private void MoveHand()
    {
        float degree =  (float)(90f +  360f * ( timer.GetTimerValue() / totalTimePhase));
        
        clockHandTransform.transform.rotation = Quaternion.Euler(0, 0, -(degree - 90f));

    }
    
    public void ReceiveSound(InstrumentType type)
    {
        Debug.Log( timer.GetTimerValue());
        if (_isBeating)
        {
            for (int i = 0 ; i< instrumentNotes.Count; i++)
            {
                var t = instrumentNotes[i];

                if (Mathf.Abs( (t.correctTime - (float)timer.GetTimerValue()) ) <=  offsetTime)
                {
                    if (_isBeatNotes[i])
                    { 
                        
                        Debug.Log("Is Beated index so reset");
                        ResetBeat();
                    }
                    else if(instrumentNotes[i].type == type)
                    {
                        Debug.Log("Hit at good time so true");
                        _isBeatNotes[i] = true;
                        return;
                    }
                    else
                    {
                        Debug.Log("Ingore");
                    }
                }
                else
                {
                    if (instrumentNotes[i].type == type && _isBeatNotes[i] == false)
                    {
                        Debug.Log("OffTime so reset");
                        ResetBeat();
                    }
                    else
                    {
                        Debug.Log("Ingore");
                    }
                }
            }
            
            Debug.Log("Nothing was hit");
            ResetBeat();
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
                    Debug.Log("Start First Beat");
                    StartFirstBeat();
                    
                    _isBeatNotes[i] = true;
                }
            }
        }
        
    }

    void Update()
    {
        double currentTime = timer.GetTimerValue();
        if (currentTime >= totalTimePhase)
        {
            if (CheckWinning())
            {
                Debug.Log("Win");
                
                // _loopCounter++;
                // if (_loopCounter == 0)
                // {
                //     Debug.Log("Check for second time");
                //     StartFirstBeat();
                // }
                // else
                // {
                //     Debug.Log("Win");
                // }
            }
            else
            {
                ResetBeat();
            }
        }
        else 
        {
            
            for (int i =0; i < instrumentNotes.Count; i++)
            {
                //check every note before the next note if it has been confirm to get all and not over delay
                
                var note = instrumentNotes[i];
                if (note.correctTime + offsetTime < currentTime) //Not counting for same correctTime that is nearly happen
                {
                    if (_isBeatNotes[i] == false)
                    {
                        Debug.Log("Miss a note");
                        ResetBeat();
                        break;
                    }
                }
                else break;
            }
        }
        MoveHand();
    }

}
