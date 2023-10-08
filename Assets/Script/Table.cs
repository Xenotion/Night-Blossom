using UnityEngine;

public class Table : MonoBehaviour
{
    public Transform[] itemSlots;
    public Timer timer; // Reference to Timer script
    public Light directionalLight; // Reference to your directional light
    public Color colorWhenItemPlaced = Color.red; // The color you want the light to change to when the object is placed

    private GameObject[] itemsOnTable;
    private bool itemPlaced = false; // Track if any item has been placed or not

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

                if (!itemPlaced)
                {
                    timer.StartTimer(); // Activate the timer on the first item placement
                    ChangeLightColor();  // Change the light color
                    itemPlaced = true;
                }

                return true;
            }
        }
        return false; // Table is full
    }

    void ChangeLightColor()
    {
        if (directionalLight != null)
        {
            directionalLight.color = colorWhenItemPlaced;
        }
    }
}
