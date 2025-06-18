using UnityEngine;
using UnityEngine.AI;

public class sc_UnitMoveNoNav : MonoBehaviour
{
    Camera cam;
    public LayerMask ground;

    private Rigidbody rb;

    public void Start()
    {
        cam = Camera.main;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                
            }
        }
    }
}
