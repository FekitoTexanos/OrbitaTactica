using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class sc_ModelsSettings : ScriptableObject
{
    public GameObject modelParent;

    public ListOfModels[] Models;

    [System.Serializable]
    public class ListOfModels
    {
        public GameObject Model;
        //[Range(0f, 1f)]
        public float minHeight =1;
        [Range(1, 200)]
        public int arrayLenght = 200;
        public Material modelMat;
        public Vector2 minmaxSize = new Vector2(1, 1);
        public float spaceNeeded = 1;
    }
}
