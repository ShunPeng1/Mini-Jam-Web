using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private GameObject instrument ;

    [SerializeField] private GameObject web;
    private LineRenderer _webLineRenderer;
    
    private bool _isDrawing = false;

    private void Start()
    {
        _webLineRenderer = web.GetComponent<LineRenderer>();
    }

    void Update()
    {
        Movement();
        DrawWeb();
    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * (Time.deltaTime * speed)); 
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * (Time.deltaTime * speed)); 
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * (Time.deltaTime * speed)); 
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * (Time.deltaTime * speed)); 
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isDrawing)
            {
                _isDrawing = false;
            }
            else
            {
                _webLineRenderer.positionCount = 2;
                _webLineRenderer.SetPosition(0, web.transform.position);
                _isDrawing = true;
            }
        }
        
        
        
    }


    private void DrawWeb()
    {
        if (_isDrawing)
        {
            _webLineRenderer.SetPosition(1, web.transform.position);
        }
        else
        {
            var position = transform.position;
            _webLineRenderer.SetPositions(new Vector3[]{position, position});
            _webLineRenderer.positionCount = 0;   
        }
        
    }
}
