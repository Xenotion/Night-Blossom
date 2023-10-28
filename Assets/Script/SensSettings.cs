using UnityEngine;
using UnityEngine.UI;

public class SensitivitySettings : MonoBehaviour
{
    public Slider sensitivitySlider;

    private void Start()
    {
        // Load saved sensitivity from PlayerPrefs or default to current value if not found
        sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", sensitivitySlider.value);
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
    }

    public void UpdateSensitivity(float newSensitivity)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", newSensitivity);
    }
}
