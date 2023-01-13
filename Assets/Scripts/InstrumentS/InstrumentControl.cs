using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum InstrumentType
{
    Red,
    Yellow,
    Green,
    Blue,
    White
}

public class InstrumentControl : MonoBehaviour
{
    [SerializeField] protected float waitToDestroy = 1f;
    [SerializeField] protected AudioSource soundSource;

    [SerializeField] private bool isRemovable = true;
    [SerializeField] private int specialID = 0;

    public InstrumentType type;
    private void Start()
    {
        soundSource = FindObjectOfType<AudioSource>();
        
        if (specialID != 0)
        {
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = specialID.ToString();
        }
    }

    protected void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && isRemovable)
        {
            if (col.gameObject.GetComponent<PlayerControl>().DashingRemoveInstrument())
            {
                Destroy(gameObject);
            }
        }

        if (col.gameObject.CompareTag("Debris"))
        {
            PlaySound(col.transform);
            SoundManager.Instance.ReceiveSound(type, specialID);
        }
    }

    protected virtual void PlaySound(Transform colPosition)
    {
        
    }

    
}
