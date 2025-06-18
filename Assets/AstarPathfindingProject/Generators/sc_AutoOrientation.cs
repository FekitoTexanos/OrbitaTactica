using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class sc_AutoOrientation : MonoBehaviour
{
    public Transform target;
    public bool isOrientated = true;
    public bool isOnlyLookingAt = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isOrientated && target != null)
        {
            transform.LookAt(target, Vector3.up);
            transform.Rotate(new Vector3(-90, 0, 0));
        }
        if (isOnlyLookingAt && target != null)
        {
            transform.LookAt(target, Vector3.up);
            
        }

    }
}
