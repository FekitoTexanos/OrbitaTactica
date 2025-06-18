using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_RigidNoiseFilter : sc_INoiseFilter
{
    sc_NoiseSettings.RigidNoiseSettings settings;
    sc_Noise noise = new sc_Noise();


    public sc_RigidNoiseFilter(sc_NoiseSettings.RigidNoiseSettings settings)
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < settings.numberOfLayers; i++)
        {
            float v = 1-Mathf.Abs(noise.Evaluate(point * frequency + settings.center)); //on pass d'une courbe sinusoidale à piquée par abs et on l'inverse pour le coté pointes vers le haut
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * settings.weigthMultiplier);
            noiseValue += v * amplitude; //v+1 * 0.5f pour passer de -1/1 du evaluate à 0/1
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
        noiseValue = Mathf.Min(noiseValue, settings.maxValue); 

        return noiseValue * settings.strenght; 
    }
}
