using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class sc_ShapeSettings : ScriptableObject
{
    public float planetRadius;
    public NoiseLayer[] noiseLayers;

    [System.Serializable]
    public class NoiseLayer
    {
        public bool enabled = true;
        public bool useFisrtLayerAsMask;
        public sc_NoiseSettings noiseSettings;
    }
}
