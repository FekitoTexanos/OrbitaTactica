using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class sc_NoiseFilterFactory 
{
    public static sc_INoiseFilter CreateNoiseFilter(sc_NoiseSettings settings)
    {
        switch(settings.filterType)
        {
            case sc_NoiseSettings.FilterType.Simple:
                return new sc_SimpleNoiseFilter(settings.simpleNoiseSettings);
            case sc_NoiseSettings.FilterType.Rigid:
                return new sc_RigidNoiseFilter(settings.rigidNoiseSettings);
        }
        return null;
    }
}
