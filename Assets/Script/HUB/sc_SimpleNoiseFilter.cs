using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class sc_SimpleNoiseFilter : sc_INoiseFilter
{
    sc_NoiseSettings.SimpleNoiseSettings settings;
    sc_Noise noise = new sc_Noise();


    public sc_SimpleNoiseFilter(sc_NoiseSettings.SimpleNoiseSettings settings )
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;

        for(int i = 0; i<settings.numberOfLayers; i++)
        {
            float v = noise.Evaluate(point*frequency + settings.center);
            noiseValue += (v + 1) * 0.5f * amplitude; //v+1 * 0.5f pour passer de -1/1 du evaluate à 0/1
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - settings.minValue);

        noiseValue = Mathf.Min(settings.maxValue, noiseValue);
        /*if (noiseValue > settings.maxValue)
            noiseValue = settings.maxValue;*/

        return noiseValue * settings.strenght;
    }
}
