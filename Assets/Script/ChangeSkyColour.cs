using UnityEngine;
using System.Collections;
public class PickupableObject : MonoBehaviour
{
    public Light directionalLight; // Reference to your directional light
    public Color colorWhenPickedUp = Color.red; // The color you want the light to change to when the object is picked up
    public Transform handPosition; // Reference to the player's hand position

    private bool hasBeenPickedUp = false;
    private float colorChangeDuration = 300.0f; // 5 minutes in seconds
    private float elapsedTime = 0.0f;

    void Update()
    {
        if (!hasBeenPickedUp && transform.parent == handPosition)
        {
            hasBeenPickedUp = true;
            StartCoroutine(ChangeLightColorOverTime());
            Debug.Log("Start");
            // Optionally, you can disable this script after the color change to save resources.
            //this.enabled = false;
        }
    }

    IEnumerator ChangeLightColorOverTime()
    {
        Color startColor = directionalLight.color;
        Color targetColor = colorWhenPickedUp;

        while (elapsedTime < colorChangeDuration)
        {
            // Calculate the new color based on the elapsed time.
            float t = elapsedTime / colorChangeDuration;
            directionalLight.color = Color.Lerp(startColor, targetColor, t);

            elapsedTime += Time.deltaTime;
            Debug.Log(elapsedTime);

            yield return null; // Wait for the next frame.
        }

        // Ensure the color ends up as the target color.
        directionalLight.color = targetColor;
    }
}
