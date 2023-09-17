using UnityEngine;

public class TableManager : MonoBehaviour
{
    [System.Serializable]
    public class ItemSlot
    {
        public string itemName;         // Name of the item that fits this slot
        public Transform slotTransform; // Where the item gets placed
        public bool isOccupied;         // Is something placed here already?
    }

    public ItemSlot[] itemSlots = new ItemSlot[4];
    public GameObject[] itemPrefabs;  // An array of prefabs corresponding to the 4 items.

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Assuming items are tagged "Item" and have the name set correctly.
                if (hit.transform.CompareTag("PickupObject"))
                {
                    PlaceItemOnTable(hit.transform.name);
                }
            }
        }
    }

    public Vector3 itemPositionOffset = new Vector3(0f, 5f, 0f);  // Adjust as necessary for your table's height and preferred position

    public void PlaceItemOnTable(string itemName)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].itemName == itemName && !itemSlots[i].isOccupied)
            {
                GameObject prefab = GetPrefabByName(itemName);
                if (prefab)
                {
                    Instantiate(prefab, itemSlots[i].slotTransform.position, Quaternion.identity);
                    itemSlots[i].isOccupied = true;
                }
                break;
            }
        }
    }

    private GameObject GetPrefabByName(string name)
    {
        foreach (GameObject prefab in itemPrefabs)
        {
            if (prefab.name == name)
            {
                return prefab;
            }
        }
        return null;
    }
}
