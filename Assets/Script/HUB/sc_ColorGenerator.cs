//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class sc_ColorGenerator
{

    sc_ColorSettings settings;
    Texture2D texture;
    const int textureResolution = 50;
    sc_INoiseFilter biomeNoiseFilter;

    public void UpdateSettings( sc_ColorSettings settings )
    {
        this.settings = settings;
        if(texture == null || texture.height != settings.biomeColorsSettings.biomes.Length)
            texture = new Texture2D(textureResolution, settings.biomeColorsSettings.biomes.Length);

        biomeNoiseFilter = sc_NoiseFilterFactory.CreateNoiseFilter(settings.biomeColorsSettings.noiseSettings);
    }

    public void UpdateElevation(sc_MinMax elevationMinMax)
    {
        settings.planetMat.SetVector("_MinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
    {
        float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
        heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - settings.biomeColorsSettings.noiseOffset)*settings.biomeColorsSettings.noiseStrenght;
        float biomeIndex = 0;
        int numBiomes = settings.biomeColorsSettings.biomes.Length;

        for(int i = 0; i < numBiomes; i++)
        {
            if (settings.biomeColorsSettings.biomes[i].startHeight < heightPercent)
            {
                biomeIndex = i;
            }
            else
                break;
        }
        return biomeIndex / Mathf.Max(1, numBiomes-1);
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[texture.width * texture.height];

        int colorIndex = 0;
        foreach (var biome in settings.biomeColorsSettings.biomes)
        {
            for (int i = 0; i < textureResolution; i++)
            {
                Color gradientColor = biome.gradient.Evaluate(i / (textureResolution - 1f)); //on divise pour rester entre 0 & 1
                Color tintCol = biome.tint;
                colors[colorIndex] = gradientColor * (1 - biome.tintPercent) + tintCol * biome.tintPercent;
                colorIndex++;
            }
        }
            texture.SetPixels(colors);
            texture.Apply();
            settings.planetMat.SetTexture("_Gradient", texture);
    }

}