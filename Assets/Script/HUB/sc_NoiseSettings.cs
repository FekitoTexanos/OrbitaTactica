using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[System.Serializable]
public class sc_NoiseSettings
{
    [Tooltip("Rigid is for sharp edges")]
    public enum FilterType { Simple, Rigid};
    public FilterType filterType;

    [Header("Using when the filter type is Simple")]
    public SimpleNoiseSettings simpleNoiseSettings;
    [Header("Using when the filter type is Rigid")]
    public RigidNoiseSettings rigidNoiseSettings;

    [System.Serializable]
    public class SimpleNoiseSettings
    {
        public float strenght = 1;
        [Range(1, 8)]
        public int numberOfLayers = 1;
        public float baseRoughness = 1;
        [Tooltip("Details of the BaseRoughness, usable with more than 1 layer")]
        public float roughness = 2;
        [Tooltip("Control the roughness, usable with more than 1 layer")]
        public float persistence = .5f;
        public Vector3 center;
        [Tooltip("To create seas")]
        public float minValue;
        [Tooltip("To create flat on top of mountains")] [Range(0, 1)]
        public float maxValue = 1;
    }

    [System.Serializable] 
    public class RigidNoiseSettings : SimpleNoiseSettings
    {
        public float weigthMultiplier;
    }

}
