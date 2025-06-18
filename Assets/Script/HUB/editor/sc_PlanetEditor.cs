using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(sc_Planet))]
public class sc_PlanetEditor : Editor
{
    sc_Planet planet;
    Editor shapeEditor;
    Editor colorEditor;
    Editor modelEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if(check.changed)
            {
                planet.GeneratePlanet();
            }
        }

        if(GUILayout.Button("Generate Planet"))
        {
            planet.GeneratePlanet();
        }


        if (GUILayout.Button("Generate Meshes"))
        {
            planet.GenrerateModels();
        }

        if(GUILayout.Button("Delete Meshes"))
        {
            planet.DestroyModels();
        }

        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFolded, ref shapeEditor);
        DrawSettingsEditor(planet.colorSettings, planet.OnColorSettingsUpdated, ref planet.colorSettingsFolded, ref colorEditor);
        DrawSettingsEditor(planet.modelsSettings, planet.OnModelSettingsUpdated, ref planet.modelSettingsFolded, ref modelEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if (check.changed)
                    {
                        if (onSettingsUpdated != null)
                        {
                            onSettingsUpdated();
                        }
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        planet = (sc_Planet)target;
    }
}
