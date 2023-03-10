using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using TMPro;
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
        public int specialID = 0;
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

            if (typeIndex == 4 || typeIndex == -1)
            {
                return;
            }

            float xPos = -ranges[typeIndex] * Mathf.Cos(degree* Mathf.Deg2Rad);
            float yPos = ranges[typeIndex] * Mathf.Sin(degree * Mathf.Deg2Rad);
            //Quaternion rotation = Quaternion.Euler(0, 0, -(degree - 90f));
            GameObject instantiate = Instantiate(eggPrefabs[typeIndex], new Vector2(xPos, yPos), Quaternion.identity, circleHeatMap);

            if (t.specialID != 0)
            {
                instantiate.GetComponentInChildren<TextMeshProUGUI>().text = t.specialID.ToString();
            }
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
            case InstrumentType.Red: //NOT USED THIS
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

    #region CheckWin
    
    private List<bool> _isBeatNotes;
    private bool _isBeating = false;
    private int _loopCounter = 0;


    private void ResetBeat()
    {
        timer.SetTimerValue(0);
        timer.timerState = TimerState.Disabled;

        _isBeating = false;
    }
    
    private void StartFirstBeat()
    {
        timer.RestartTimer();
        timer.finishTime = totalTimePhase;
        
        _isBeating = true;
        
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

    public void ReceiveSound(InstrumentType type, int specialID)
    {
        if (type == InstrumentType.White) // Ignore White type
        {
            return;
        }
        
        if (_isBeating)
        {
            for (int i = 0 ; i< instrumentNotes.Count; i++)
            {
                var t = instrumentNotes[i];

                if (Mathf.Abs( (t.correctTime - (float)timer.GetTimerValue()) ) <=  offsetTime)
                {
                    if(_isBeatNotes[i]==false && instrumentNotes[i].type == type && (specialID == instrumentNotes[i].specialID || instrumentNotes[i].specialID == 0))
                    {
                        Debug.Log("Hit at good time so true");
                        _isBeatNotes[i] = true;
                        return;
                    }
                    else if (_isBeatNotes[i]  && instrumentNotes[i].type == type && (specialID == instrumentNotes[i].specialID|| instrumentNotes[i].specialID == 0))
                    {
                        Debug.Log("Is Beated index so reset");
                        ResetBeat();
                        
                    }
                    else
                    {
                        Debug.Log("Ingore");
                    }
                }
                else
                {
                    if (instrumentNotes[i].type == type  && _isBeatNotes[i] == false)
                    {
                        Debug.Log("OffTime so reset");
                        ResetBeat();
                        return;
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

    private bool LinearSearchAllNote(double currentTime)
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
                    return false;
                }
            }
            else return true;
        }

        return true;
    }
    

    #endregion
    
    void Update()
    {
        double currentTime = timer.GetTimerValue();
        if (currentTime >= totalTimePhase)
        {
            if (CheckWinning())
            {
                Invoke( nameof(WinLevel) , 1f);
            }
            else
            {
                ResetBeat();
            }
        }
        else
        {
            LinearSearchAllNote(currentTime);
        }
        
        
        MoveHand();
    }

    void WinLevel()
    {
        SceneManager.Instance.GetNextScene();
    }

}
