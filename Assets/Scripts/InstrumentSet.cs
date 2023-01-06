using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InstrumentSet : MonoBehaviour
{
    public static InstrumentSet Instance; 
    void Start()
    {
        Instance = this;
        _instruments = new List<GameObject>();
    }
    
    [SerializeField] private GameObject instrument;
    [SerializeField] private float instrumentsWidth = 0.1f;
        
    private List<GameObject> _instruments;
    public void PlaceInstrument(Vector3 firstPosition, Vector3 secondPosition)
    {
        Vector3 middlePosition = (firstPosition + secondPosition) / 2;
        Vector3 deltaPosition = (secondPosition - firstPosition);
        if(deltaPosition.magnitude == 0) return;
        Vector3 size =  new Vector3( deltaPosition.magnitude  ,instrumentsWidth * (deltaPosition.y >=0 ? 1: -1),0) ;
        
        Vector3 rotation = new Vector3(0,0, Mathf.Acos( deltaPosition.normalized.x * (deltaPosition.y >=0? 1:-1 ) / deltaPosition.normalized.magnitude  )* Mathf.Rad2Deg);

        GameObject instantiated = Instantiate(instrument ,middlePosition, Quaternion.Euler(rotation));
        instantiated.transform.localScale = size;
        
        _instruments.Add(instantiated);
    }


    /*
    public float distance = 50f;
    //replace Update method in your class with t$$anonymous$$s one
    void FixedUpdate () 
    {    
        //if mouse button (left hand side) pressed instantiate a raycast
        if(Input.GetMouseButtonDown(0))
        {
            //create a ray cast and set it to the mouses cursor position in game
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast (ray, out RaycastHit raycastHit, distance)) 
            {
                //draw invisible ray cast/vector
                Debug.DrawLine (ray.origin, raycastHit.point);
                //log $$anonymous$$t area to the console
                Debug.Log(raycastHit.point);
                                   
            }    
        }    
    }
    */
}

