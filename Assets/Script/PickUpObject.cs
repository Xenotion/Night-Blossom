using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public Transform handPosition; // This is where the object will be placed when picked up.
    public float interactionRange = 3f; // The range at which player can interact with objects.

    private Camera playerCamera; // Player's camera for raycasting.

    private void Start()
    {
        // Assuming the script is on the same GameObject as the camera.
        playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange))
        {
            // Check if we hit a pickup object and if F is pressed
            if (hit.collider.CompareTag("PickupObject") && Input.GetKeyDown(KeyCode.F))
            {
                PickUp(hit.collider.gameObject);
            }
        }
    }

    void PickUp(GameObject pickedObject)
    {
        // Set the object's parent to the hand's transform.
        pickedObject.transform.SetParent(handPosition);

        // Position the object at the hand's location.
        pickedObject.transform.position = handPosition.position;

        // Optionally, set its local rotation to a preferred default.
        pickedObject.transform.localRotation = Quaternion.identity;
    }
}

