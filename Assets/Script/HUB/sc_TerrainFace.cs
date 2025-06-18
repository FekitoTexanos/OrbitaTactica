using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_TerrainFace
{
    sc_ShapeGenerator shapeGenerator;
    public Mesh mesh;
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;

    public sc_TerrainFace(sc_ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1)*(resolution - 1) *2* 3]; // (resolution-1)² pour avoir le nbr de carré avec la resolution, * 2 pour le nbr de triangle * 3 car 1 triangle = 3 vertices
        int triIndex = 0;
        Vector2[] uv = mesh.uv;


        for(int y = 0; y < resolution; y++)
        {
            for(int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                vertices[i] = shapeGenerator.CalculatePointOnPLanet(pointOnUnitSphere);

                if(x!= resolution-1 && y!= resolution-1)
                {
                    triangles[triIndex] = i; //exemples avec resolution de 4 : 1er triangle, points à 0 puis 5 puis 4
                    triangles[triIndex+1] = i+resolution+1; //point 5 donc resolution +1
                    triangles[triIndex+2] = i+resolution; //point 4 donc resolution

                    triangles[triIndex + 3] = i; //exemples avec resolution de 4 : 2eme triangle, points à 0 puis 1 puis 5
                    triangles[triIndex + 4] = i + 1; //point 1 donc +1
                    triangles[triIndex + 5] = i + resolution + 1; //point 5 donc resolution +1

                    triIndex += 6;

                }

            }
        }

        mesh.Clear(); //pour pas se retrouver avec pleins de mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.uv = uv;

    }

    public void UpdateUVs(sc_ColorGenerator colorGenerator)
    {
        Vector2[] uv = new Vector2[resolution * resolution];

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                uv[i] = new Vector2(colorGenerator.BiomePercentFromPoint(pointOnUnitSphere), 0);
            }
            mesh.uv = uv;
        }
    }
}
