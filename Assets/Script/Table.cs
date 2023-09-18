using UnityEngine;

public class Table : MonoBehaviour
{
    public Transform[] itemSlots; // Assign ItemSlot1, ItemSlot2, etc. to this in the inspector

    private GameObject[] itemsOnTable;

    private void Start()
    {
        itemsOnTable = new GameObject[itemSlots.Length];
    }

    public bool PlaceItemOnTable(GameObject item)    
    {
        for (int i = 0; i < itemsOnTable.Length; i++)
        {
            if (itemsOnTable[i] == null)
            {
                itemsOnTable[i] = item;
                item.SetActive(true);
                item.transform.position = itemSlots[i].position;
                return true;
            }
        }
        return false; // Table is full
    }
}
