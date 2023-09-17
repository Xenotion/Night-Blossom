using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public Transform handPosition; // Where the object will be placed when picked up
    public float interactionRange = 3f; // Range at which player can interact with objects
    private GameObject heldItem; // Reference to the currently held item

    private Camera playerCamera; // Player's camera for raycasting
    private TableManager tableManager; // Reference to the table manager script

    private void Start()
    {
        // Assuming the script is on the same GameObject as the camera
        playerCamera = GetComponentInChildren<Camera>();

        // Get reference to the TableManager script
        tableManager = FindObjectOfType<TableManager>();
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange))
        {
            // Check if we hit an object
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (hit.collider.CompareTag("PickupObject") && heldItem == null)
                {
                    // Pick up the object
                    PickUp(hit.collider.gameObject);
                }
                else if (heldItem != null && hit.collider.CompareTag("Table"))
                {
                    // Place the object on the table
                    PlaceItemOnTable();
                }
            }
        }
    }

    void PickUp(GameObject pickedObject)
    {
        heldItem = pickedObject;

        // Set the object's parent to the hand's transform
        pickedObject.transform.SetParent(handPosition);

        // Position the object at the hand's location
        pickedObject.transform.position = handPosition.position;

        // Optionally, set its local rotation to a preferred default
        pickedObject.transform.localRotation = Quaternion.identity;
    }

    void PlaceItemOnTable()
    {
        if (heldItem == null) return;

        string itemName = heldItem.name;
        tableManager.PlaceItemOnTable(itemName);
        Destroy(heldItem);
        heldItem = null;
    }
}
