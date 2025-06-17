using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class sc_Unit_Selection_Manager_TV : MonoBehaviour
{
    public static sc_Unit_Selection_Manager_TV instance { get; set; }
    
    public LayerMask clickable;
    public LayerMask ground;
    public GameObject groundMarker;

    public List<GameObject> allUnitList = new List<GameObject>();
    public List<GameObject> unitSelected = new List<GameObject>();

    private Camera cam;
    private void Awake()
    {
        if(instance !=null & instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void Start()
    {
        cam = Camera.main;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //If we hit units (or other clickable objects)
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MultiSelected(hit.collider.gameObject);
                }
                else
                {
                    SelectByClicking(hit.collider.gameObject);
                }
            }
            else //if we dont hit an unit (ground)
            {
                if (Input.GetKey(KeyCode.LeftShift) == false) //if we dont hit an unit meanwhile multiselected method on, we dont unselect
                {
                    DeselectAll();
                }
                
            }
        }

        if (Input.GetMouseButtonDown(1) && unitSelected.Count >0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //if we hit ground
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {

                groundMarker.transform.position = hit.point;
                groundMarker.SetActive(false);
                groundMarker.SetActive(true);
            }
        }


        }

    private void MultiSelected(GameObject unit)
    {
        if (unitSelected.Contains(unit) == false)
        {
            unitSelected.Add(unit);
            SelectUnitToMove(unit, true);
        }
        else
        {
            SelectUnitToMove(unit, false);
            unitSelected.Remove(unit);
            
        }
    }

    public void DeselectAll()
    {
        foreach(var unit in unitSelected)
        {
            SelectUnitToMove(unit, false);
        }
        groundMarker.SetActive(false);
        unitSelected.Clear();
    }

    private void SelectByClicking(GameObject unit)
    {
        DeselectAll(); //we unselect all the previous selected units
        unitSelected.Add(unit);
        SelectUnitToMove(unit, true);
    }

    private void EnableMoveUnit(GameObject unit, bool triggerMove)
    {
        unit.GetComponent<sc_SimpleUnit_Move_TV>().enabled = triggerMove;
    }

    private void SelectUnitToMove(GameObject unit, bool triggerMove)
    {
        TriggerIndicatorUnit(unit, triggerMove);
        EnableMoveUnit(unit, triggerMove);
    }

    public void TriggerIndicatorUnit(GameObject unit, bool isVisible)
    {
        unit.transform.GetChild(0).gameObject.SetActive(isVisible);
    }

    internal void DragSelect(GameObject unit)
    {
        if (unitSelected.Contains(unit) == false)
        {
            unitSelected.Add(unit);
            TriggerIndicatorUnit(unit, true);
            EnableMoveUnit(unit, true);
        }
    }
}
