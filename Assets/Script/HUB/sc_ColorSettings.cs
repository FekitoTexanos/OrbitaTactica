using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class sc_ColorSettings : ScriptableObject
{
    public Material planetMat;
    public BiomeColorsSettings biomeColorsSettings;

    [System.Serializable]
    public class BiomeColorsSettings
    {
        public Biome[] biomes;
        public sc_NoiseSettings noiseSettings;
        public float noiseOffset;
        public float noiseStrenght;

        [System.Serializable]
        public class Biome
        {
            public Gradient gradient;
            public Color tint;
            [Range(0,1)]
            public float startHeight;
            [Range(0,1)]
            public float tintPercent;
        }
    }
}
