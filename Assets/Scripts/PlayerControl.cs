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
    private Vector3 _firstPosition, _secondPosition;
    
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
                PlaceInstrument();
                _isDrawing = false;
            }
            else
            {
                _webLineRenderer.positionCount = 2;
                _firstPosition = web.transform.position;
                _webLineRenderer.SetPosition(0, _firstPosition);
                _isDrawing = true;
            }
        }
        
        
        
    }


    private void DrawWeb()
    {
        if (_isDrawing)
        {
            _secondPosition = web.transform.position;
            _webLineRenderer.SetPosition(1,_secondPosition);
        }
        else
        {
            var position = transform.position;
            _webLineRenderer.SetPositions(new Vector3[]{position, position});
            _webLineRenderer.positionCount = 0;   
        }
        
    }

    private void PlaceInstrument()
    {
        Vector3 middlePosition = (_firstPosition + _secondPosition) / 2;
        Vector3 deltaPosition = (_secondPosition - _firstPosition);
        if(deltaPosition.magnitude == 0) return;
        Vector3 size =  new Vector3( deltaPosition.magnitude  ,(deltaPosition.y >=0 ? 1: -1),0) ;
        
        Vector3 rotation = new Vector3(0,0, Mathf.Acos( deltaPosition.normalized.x * (deltaPosition.y >=0? 1:-1 ) / deltaPosition.normalized.magnitude  )* Mathf.Rad2Deg);
        Debug.Log(rotation);
        
        GameObject instantiated = Instantiate(instrument ,middlePosition, Quaternion.Euler(rotation));
        instantiated.transform.localScale = size;
        
    }
}
