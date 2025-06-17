using UnityEngine;

public class sc_Unit_Selection_TV : MonoBehaviour
{

    void Start()
    {
        sc_Unit_Selection_Manager_TV.instance.allUnitList.Add(gameObject);
    }

    private void OnDestroy()
    {
        sc_Unit_Selection_Manager_TV.instance.allUnitList.Remove(gameObject);

    }

}
