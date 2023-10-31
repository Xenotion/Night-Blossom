using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class HealthAndVignette : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to your PlayerHealth script
    public Image[] hearts; // Assign your heart images in the inspector, make sure it's in order

    public PostProcessVolume postProcessVolume;
    Vignette vignetteLayer;

    float vignetteIntensityHealth2 = 0.45f; // Intensity when health is 2
    float vignetteIntensityHealth1OrBelow = 0.55f; // Intensity when health is 1 or below

    float pulseSpeed = 1.5f; // Speed of the pulsating effect
    float time = 0f; // Time variable for pulsation

    void Start()
    {
        if (postProcessVolume.profile.TryGetSettings(out vignetteLayer))
        {
            vignetteLayer.active = false; // Initially, vignette effect is turned off
        }
    }

    void Update()
    {
        UpdateHealthUI();
        UpdateVignette();
    }

    void UpdateHealthUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth.health)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    void UpdateVignette()
    {
        if (vignetteLayer != null && playerHealth != null)
        {
            float playerHealthValue = playerHealth.getHealth();

            // Check the player's health and adjust the vignette effect accordingly
            bool healthCritical = IsHealthCritical(playerHealthValue);
            vignetteLayer.active = healthCritical;

            if (playerHealthValue == 2)
            {
                SetVignetteIntensity(vignetteIntensityHealth2);
            }
            else if (playerHealthValue <= 1)
            {
                SetVignetteIntensity(GetPulsatingIntensity(vignetteIntensityHealth1OrBelow));
            }
        }
    }
    bool IsHealthCritical(float healthValue)
    {
        return healthValue <= 2;
    }

    // Pulsing effect 
    void SetVignetteIntensity(float intensity)
    {
        vignetteLayer.intensity.value = intensity;
    }
    float GetPulsatingIntensity(float baseIntensity)
    {
        time += Time.deltaTime * pulseSpeed;
        return baseIntensity + Mathf.Sin(time) * 0.1f; 
    }
}
