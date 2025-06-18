using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class sc_PlanetMove : MonoBehaviour
{
    [Header("Planets (mettre en 0 la lune)")]
    public GameObject[] Allplanet;
    private GameObject planet;

    [Header("Planets DefaultZoom")]
    public float LuneZoomDefault = 50;
    public float PotagerZoomDefault = 75;
    public float DesertZoomDefault = 75;
    public float CrababordZoomDefault = 100;

    [Header("ChangementDePlanet")]
    public float speedToChange;
    //public float speedToBegin;
    //public float distanceFromPlanet;

    [Header("Rotate")]
    public float velocity;
    public float sensX, sensY, inertiaDamping, maxSpeed;

    bool isMouseDown;
    Vector3 lastmousePos, currentRotationVelocity;

    [Header("Zoom")]
    public float sensZoom;
    public float minZoom, maxZoom;
    float globalzoom;
    float localzoom;
    public float speedToChangeZoom;

    Camera cam;
    private bool _isOkToZoom;
    private float zoomDirection;
    private bool IsTuto = false;
    
    

  
    void Start()
    {
        _isOkToZoom = false;
        cam = transform.GetChild(1).GetComponent<Camera>();
        globalzoom = cam.fieldOfView;
        localzoom = globalzoom;
        zoomDirection = globalzoom;


        

        //speedSave = speedToChange;
    }

    void Update()
    {
        
        #region(PlanetRotation)


            if (Input.GetMouseButtonDown(0))
            {
                isMouseDown = true;
                lastmousePos = Input.mousePosition;

            }

            if (Input.GetMouseButton(0) && isMouseDown)
            {
                Vector3 delta = Input.mousePosition - lastmousePos;

                //rotation en fonction dues mouvements
                float rotaLR = -delta.y * sensY * Time.deltaTime;
                float rotaUD = delta.x * sensX * Time.deltaTime;

                transform.Rotate(Vector3.up, rotaUD);
                transform.Rotate(Vector3.right, rotaLR);

                //stocker la vitesse pour l'inertie
                currentRotationVelocity = Vector3.Lerp(currentRotationVelocity, new Vector3(rotaLR * velocity, rotaUD * velocity, 0), 0.5f);
                currentRotationVelocity = Vector3.ClampMagnitude(currentRotationVelocity, maxSpeed);

                lastmousePos = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isMouseDown = false;
            }

            if (!isMouseDown)
            {
                //appliquer une rotation basée sur l'inertie
                transform.Rotate(Vector3.up, currentRotationVelocity.y);
                transform.Rotate(Vector3.right, currentRotationVelocity.x);

                //réduire la vitesse de rotation
                currentRotationVelocity *= Mathf.Pow(inertiaDamping, Time.deltaTime * 60f);


                if (currentRotationVelocity.magnitude < 0.05f)
                    currentRotationVelocity = Vector3.zero;


            }

            #endregion      
        ChangeZoom();        
        MovePlanet(planet);
    }

    public void ChangePlanet(GameObject Changeplanet) // change les mix max
    {   
        planet = Changeplanet;
        transform.SetParent(planet.transform);
        if(planet.name == "HomeSystem")
        {
            
            //cam.transform.localPosition = new Vector3(0,0,-2000);
            //transform.localRotation = Quaternion.Euler(90, 0, 0);
            //_isOkToZoom = false;
           
            maxZoom = 50;
            minZoom = 50;
            

        } 
        else if(planet.name == "PlanetLune")
        {
            maxZoom = 50;
            minZoom = 15;
            
        }
        else if(planet.name == "PlanetCrababord")
        {
            maxZoom = 110;
            minZoom = 70;
            

        }
        else if(planet.name == "PlanetPotager" || planet.name == "PlanetDesert")
        {
            maxZoom = 80;
            minZoom = 35;
            
        }
        
        
    }
    public void MovePlanet(GameObject planet)
    {

        gameObject.transform.position = Vector3.MoveTowards(transform.position, planet.transform.position, speedToChange * Time.deltaTime);
        //cam.transform.LookAt(planet.transform.localPosition);
        if(planet.name == "HomeSystem")
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(90, 0, 0), Time.deltaTime);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        
        foreach( var trigger in Allplanet)
        {
            if(other.gameObject.name == trigger.name)
            {                
                _isOkToZoom = true; //Active le localZoom car la cam est lock sur une planet
                
            }
        }
    }
  


    public void ChangeZoom()
    {
        if (!_isOkToZoom)
        {
            
            globalzoom = Mathf.Lerp(globalzoom, zoomDirection, speedToChangeZoom * Time.deltaTime);            
            cam.fieldOfView = globalzoom;
            localzoom = globalzoom; // met a jour la variable localZoom en fonction de ou était le globalzoom
            
        }
        else if(_isOkToZoom)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");           
            localzoom -= scroll * sensZoom;
            localzoom = Mathf.Clamp(localzoom, minZoom, maxZoom);
            
            cam.fieldOfView = localzoom;
            
        }

    }
    public void ChangeZoomDirection(float newZoomDirection) //Update la valeur du zoom à atteindre (car chaque planet à un zoom par défaut suivant leur taille)
    {
        zoomDirection = newZoomDirection;
        globalzoom = localzoom; //donne la valeur de base du globalzoom à jour enfonction du localzoom avant de se diriger vers la nouveau zoomDirection
        _isOkToZoom = false; // desactive le localzoom le temps du voyage
        


    }
    public void AuCommencement()
    {
        ChangePlanet(planet);
  
    }

    public bool CurretPlanet(GameObject NewPlanet)
    {
        if(NewPlanet.name == planet.name)
        {
            return false;
        }
        else
        {
            return true;           
        }
    }
}
