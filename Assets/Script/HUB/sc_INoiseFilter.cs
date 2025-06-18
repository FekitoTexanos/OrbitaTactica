using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface sc_INoiseFilter 
{
    float Evaluate(Vector3 point);
}
