using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private TrailRenderer _trailRenderer;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float rotatedSpeed;

    
    [Header("Drawing Instrument")]
    [SerializeField] private GameObject abdomen;
    
    private bool _isDrawing = false;
    private Vector3 _firstPosition, _secondPosition;

    
    [Header("Dashing")] 
    [SerializeField] private float dashingDuration = 1f;
    [SerializeField] private float dashingBoost;
    [SerializeField] private float dashingCooldown = 2f;
    [SerializeField] private bool canDash = true;
    private bool _isDashing = false;
    
    private void Start()
    {
        
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _trailRenderer = gameObject.GetComponent<TrailRenderer>();
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
        if (_isDashing) return;
        
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
        
        if (velocity == Vector3.zero) return;
        
        
        Quaternion lookRotation = quaternion.LookRotation(Vector3.forward, velocity.normalized);
        lookRotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotatedSpeed * Time.fixedDeltaTime);
        //_rigidbody2D.MoveRotation(lookRotation);
        transform.rotation = lookRotation;

        if (Input.GetKey(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dashing());
        }
        
    }

    private IEnumerator Dashing()
    {
        _isDashing = true;
        canDash = false;

        _rigidbody2D.velocity *= dashingBoost;
        DashingVisualize();

        yield return new WaitForSeconds(dashingDuration);
        
        _isDashing = false;
        DashingVisualize();
        yield return new WaitForSeconds(dashingCooldown);

        canDash = true;

    }

    private void DashingVisualize()
    {
        _trailRenderer.emitting = _isDashing;
    }

    public bool DashingRemoveInstrument()
    {
        if (_isDashing)
        {
            _rigidbody2D.velocity = Vector2.zero;
        }

        return _isDashing;
    }
    
    private void InputWeb()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isDrawing)
            {
                abdomen.SetActive(false);    
                _isDrawing = false;
                
                InstrumentManager.Instance.DestroyLastInserted();
                InstrumentManager.Instance.PlaceInstrument(_firstPosition,_secondPosition, false);
            }
            else
            {
                var position = abdomen.transform.position;
                _firstPosition = position;
                _secondPosition = position;
                //_webLineRenderer.SetPosition(0, _firstPosition);
                //_webLineRenderer.SetPosition(1, _secondPosition);
                abdomen.SetActive(true);
                
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
            _secondPosition = abdomen.transform.position;
            //_webLineRenderer.SetPosition(1,_secondPosition);
            InstrumentManager.Instance.ReTransformOrInstantiateInLastInsert(_firstPosition,_secondPosition);
        }
        
    }

    
    
}
