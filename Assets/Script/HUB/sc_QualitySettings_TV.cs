using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sc_QualitySettings_TV : MonoBehaviour
{
    public string _qualityName;

    public static sc_QualitySettings_TV Instance;
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.Log("Current Quality Avant destruction: " + QualitySettings.names[QualitySettings.GetQualityLevel()]);
            Destroy(gameObject);
        }
        if (SceneManager.GetActiveScene().name == "PlanetHUB")
        {

            for (int i = 0; i < QualitySettings.names.Length; i++)
            {
                if (QualitySettings.names[i] == _qualityName)
                    QualitySettings.SetQualityLevel(i);
            }
        }
        else
        {
            for (int i = 0; i < QualitySettings.names.Length; i++)
            {
                if (QualitySettings.names[i] == "Ultra")
                    QualitySettings.SetQualityLevel(i);
            }
        }

        //Debug.Log("Current Quality ACTUELLE: " + QualitySettings.names[QualitySettings.GetQualityLevel()]);
    }
    

    
}
