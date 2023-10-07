using UnityEngine;

public class LightingDebug : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Ambient Light Color: " + RenderSettings.ambientLight);
        Debug.Log("Directional Light Intensity: " + RenderSettings.sun.intensity);
        Debug.Log("Directional Light Color: " + RenderSettings.sun.color);
    }
}

