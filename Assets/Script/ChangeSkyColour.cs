using UnityEngine;

public class PickupableObject : MonoBehaviour
{
    public Light directionalLight; // Reference to your directional light
    public Color colorWhenPickedUp = Color.red; // The color you want the light to change to when the object is picked up
    public Transform handPosition; // Reference to the player's hand position

    // This flag will ensure the light color change occurs only once.
    private bool hasBeenPickedUp = false;

    void Update()
    {
        // If the object's parent is the hand position and it hasn't been picked up before
        if (!hasBeenPickedUp && transform.parent == handPosition)
        {
            hasBeenPickedUp = true;
            ChangeLightColor();
            // Optionally, you can disable this script after the color change to save resources.
            this.enabled = false;
        }
    }

    void ChangeLightColor()
    {
        if (directionalLight != null)
        {
            directionalLight.color = colorWhenPickedUp;
        }
    }
}
