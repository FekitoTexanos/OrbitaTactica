using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_ShapeGenerator
{
    sc_ShapeSettings settings;
    sc_INoiseFilter[] noiseFilters;
    public sc_MinMax elevationMinMax;
    
    public void UpdateSettings(sc_ShapeSettings settings)
    {
        this.settings = settings;
        noiseFilters = new sc_INoiseFilter[settings.noiseLayers.Length];

        for(int i = 0 ; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = sc_NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
        elevationMinMax = new sc_MinMax();
    }

    public Vector3 CalculatePointOnPLanet(Vector3 PointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        if(noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(PointOnUnitSphere);
            if (settings.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for(int i =1 ; i < noiseFilters.Length; i++) //on commence à 1 car on à récup le 0 avec le if du dessus
        {
            if (settings.noiseLayers[i].enabled)
            {
                float mask = (settings.noiseLayers[i].useFisrtLayerAsMask) ? firstLayerValue : 1;
                elevation += noiseFilters[i].Evaluate(PointOnUnitSphere) * mask;
            }
        }
        elevation = settings.planetRadius * (1 + elevation);
        elevationMinMax.AddValue(elevation);
        return PointOnUnitSphere * elevation;
    }

}
