using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotatedSpeed;

    [SerializeField] private GameObject web;
    
    private LineRenderer _webLineRenderer;
    private Rigidbody2D _rigidbody2D;
    
    private bool _isDrawing = false;
    private Vector3 _firstPosition, _secondPosition;
    
    private void Start()
    {
        _webLineRenderer = web.GetComponent<LineRenderer>();
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        InputWeb();
        InputDeleteAllInstrument();
    }

    private void FixedUpdate()
    {
        Movement();
        DrawWeb();
    }

    void Movement()
    {
        Vector3 velocity = Vector3.zero;
        
        if (Input.GetKey(KeyCode.A))
        {
            velocity+= Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            velocity+= Vector3.right;
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            velocity+= Vector3.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            velocity+= Vector3.down;
        }

        _rigidbody2D.velocity = velocity * speed;

        if (velocity != Vector3.zero)
        {
            Quaternion lookRotation = quaternion.LookRotation(Vector3.forward, velocity.normalized);
            lookRotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotatedSpeed * Time.fixedDeltaTime);
            //_rigidbody2D.MoveRotation(lookRotation);
            transform.rotation = lookRotation;
        }
        
        
    }

    private void InputWeb()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isDrawing)
            {
                InstrumentManager.Instance.DestroyLastInserted();
                InstrumentManager.Instance.PlaceInstrument(_firstPosition,_secondPosition, false);
                web.SetActive(false);    
                _isDrawing = false;
            }
            else
            {
                _firstPosition = web.transform.position;
                _webLineRenderer.SetPosition(0, _firstPosition);
                _webLineRenderer.SetPosition(1, _secondPosition);
                web.SetActive(true);
                
                InstrumentManager.Instance.PlaceInstrument(_firstPosition,_secondPosition, true);
                _isDrawing = true;
            }
        }   
    }

    private void InputDeleteAllInstrument()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            InstrumentManager.Instance.DestroyAll();
        }
    }
    private void DrawWeb()
    {
        if (_isDrawing)
        {
            _secondPosition = web.transform.position;
            _webLineRenderer.SetPosition(1,_secondPosition);
            InstrumentManager.Instance.ReTransformOrInstantiateInLastInsert(_firstPosition,_secondPosition);
        }
        
    }

    
}
