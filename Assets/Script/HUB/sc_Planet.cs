using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;
    public bool autoUpdate = true;

    public sc_ShapeSettings shapeSettings;
    public sc_ColorSettings colorSettings;
    public sc_ModelsSettings modelsSettings;

    [HideInInspector]
    public bool shapeSettingsFolded;
    [HideInInspector]
    public bool colorSettingsFolded;
    [HideInInspector]
    public bool modelSettingsFolded;

    sc_ShapeGenerator shapeGenerator = new sc_ShapeGenerator();
    sc_ColorGenerator colorGenerator = new sc_ColorGenerator();
    sc_ModelsGenerator modelGenerator = new sc_ModelsGenerator();

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    sc_TerrainFace[] terrainFaces;

    [HideInInspector]
    public GameObject parentModel;

    private void Start()
    {
        Initialize();
        GenerateColors();
    }
    void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colorGenerator.UpdateSettings(colorSettings);
        modelGenerator.UpdateSettings(modelsSettings);

        if(meshFilters == null || meshFilters.Length == 0)
        {
        meshFilters = new MeshFilter[6]; // 6faces pour un carré
        }
        terrainFaces = new sc_TerrainFace[6];

        Vector3[] directions = {Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back};

        for(int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            meshFilters[i].GetComponent<MeshRenderer>().material = colorSettings.planetMat;

            terrainFaces[i] = new sc_TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
        //GenrerateModels();
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColorSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColors();
        }
    }

    public void OnModelSettingsUpdated()
    {
        /*if (autoUpdate)
        {
            Initialize();
            GenrerateModels();
        }*/
    }

    void GenerateMesh()
    {
        foreach(sc_TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }

        colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    void GenerateColors()
    {
        colorGenerator.UpdateColors();

        for(int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].UpdateUVs(colorGenerator);
            }
        }
    }

    public void GenrerateModels()
    {

            if (parentModel != null)
                DestroyImmediate(parentModel);

            if (parentModel == null)
                parentModel = Instantiate(modelsSettings.modelParent, transform);

        Initialize();
        modelGenerator.UpdateModels(terrainFaces, this);
    }

    public void DestroyModels()
    {
        if (parentModel != null)
            DestroyImmediate(parentModel);
    }

}
