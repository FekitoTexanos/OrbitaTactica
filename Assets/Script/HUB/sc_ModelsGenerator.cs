using System.Collections.Generic;
using UnityEngine;

public class sc_ModelsGenerator
{
    public sc_ModelsSettings settings;

    List<Vector3> allVertices = new List<Vector3>();

    public void UpdateSettings(sc_ModelsSettings settings)
    {
        this.settings = settings;
    }

    public void UpdateModels(sc_TerrainFace[] terrainFace, sc_Planet planet)
    {
        //faire spawn les models


        foreach (sc_TerrainFace face in terrainFace)
        {
            float lastNoiseHeight = 0;


            for (int i = 0; i < face.mesh.vertices.Length; i++)
            {
                Vector3 pos = new Vector3(face.mesh.vertices[i].x, face.mesh.vertices[i].y, face.mesh.vertices[i].z);
                int randomModel = Random.Range(0, settings.Models.Length);
                bool canBePlaced = true;

                if (Random.Range(1, (settings.Models[randomModel].arrayLenght + 1)) == 1)
                {
                    if (Vector3.Distance(planet.gameObject.transform.position, pos) > settings.Models[randomModel].minHeight + planet.shapeSettings.planetRadius)
                    {

                        foreach (Transform models in planet.parentModel.transform)
                        {
                            if (Vector3.Distance(models.position, pos) < settings.Models[randomModel].spaceNeeded)
                            {
                                canBePlaced = false;
                            }
                        }

                        if (canBePlaced)
                        {

                            //Debug.Log(Vector3.Distance(planet.gameObject.transform.position, pos));
                            GameObject model = Object.Instantiate(settings.Models[randomModel].Model, pos, Quaternion.identity, planet.parentModel.transform) as GameObject;
                            model.GetComponent<MeshRenderer>().material = settings.Models[randomModel].modelMat;
                            model.AddComponent<sc_AutoOrientation>();
                            model.GetComponent<sc_AutoOrientation>().target = planet.gameObject.transform;

                            RandomizeModelSizeRot(model, settings.Models[randomModel].minmaxSize);
                        }
                    }
                }



                /*
                if (Random.Range(1, (settings.Models[randomModel].arrayLenght+1)) == 1)
                {
                    if (Vector3.Distance(planet.gameObject.transform.position, pos) > settings.Models[randomModel].minHeight + planet.shapeSettings.planetRadius)
                    {

                        //Debug.Log(Vector3.Distance(planet.gameObject.transform.position, pos));
                        GameObject model = Object.Instantiate(settings.Models[randomModel].Model, pos, Quaternion.identity, planet.parentModel.transform) as GameObject;
                        model.GetComponent<MeshRenderer>().material = settings.Models[randomModel].modelMat;
                        model.AddComponent<sc_AutoOrientation>();
                        model.GetComponent<sc_AutoOrientation>().target = planet.gameObject.transform;

                        RandomizeModelSizeRot(model, settings.Models[randomModel].minmaxSize);
                    }
                }
                */
            }
        }

    }

    void RandomizeModelSizeRot(GameObject model, Vector2 minmaxSize)
    {
        float randmSize = Random.Range(minmaxSize.x, minmaxSize.y);
        Vector3 newSize = model.transform.localScale * (1 + randmSize / 10);
        model.transform.localScale = newSize;

        int randmRot = Random.Range(-360, 361);
        Vector3 newRot = new Vector3(0, randmRot, 0);
        model.transform.Rotate(newRot);
    }
}
