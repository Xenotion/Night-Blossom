using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public Transform handPosition;
    public float interactionRange = 3f;
    private EnemyAI enemy;

    private GameObject heldItem;

    private Camera playerCamera;
    
    private Table table;

    private void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        table = FindObjectOfType<Table>();
        enemy = FindObjectOfType<EnemyAI>();
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (hit.collider.CompareTag("PickupObject") && heldItem == null)
                {
                    PickUp(hit.collider.gameObject);
                }
                else if (heldItem != null && hit.collider.CompareTag("Table"))
                {
                    PlaceItemOnTable();
                }
            }
        }
    }

    void PickUp(GameObject pickedObject)
    {
        heldItem = pickedObject;
        pickedObject.transform.SetParent(handPosition);
        pickedObject.transform.position = handPosition.position;
        pickedObject.transform.localRotation = Quaternion.identity;

        // teleport enemy to player after object pickup
        enemy.Enrage();

        // Deactivate the object's physics if it has any, so it doesn't interfere while being held.
        Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
        }
    }

    void PlaceItemOnTable()
    {
        if (heldItem == null) return;

        // Try to place the item on the table
        bool placedSuccessfully = table.PlaceItemOnTable(heldItem);

        // If successfully placed, detach from the player's hand
        if (placedSuccessfully)
        {
            heldItem.transform.SetParent(null);
            heldItem = null;
            DebuffEnemy();
        }
    }

    // apply debuff to enemy, can add more logic in here to allow different debuffs etc.
    void DebuffEnemy(){
        enemy.Stun(10.0f);
        enemy.ResetSpeed();
    }
}
