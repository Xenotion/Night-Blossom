using UnityEngine;
using System.Collections;

public class Table : MonoBehaviour
{
    public Transform[] itemSlots;
    public Timer timer; // Reference to Timer script
    public GameObject healthUI; // Reference to the health UI
    public GameObject timerUI; // Reference to the timer UI
    public Light directionalLight; // Reference to your directional light
    public Color colorWhenItemPlaced = Color.red; // The color you want the light to change to when the object is placed
    public Color colorAtEnd = Color.white;

    // TODO: get this from Timer script?
    private float colorChangeDuration = 300.0f; // 5 minutes in seconds
    private float elapsedTime = 0.0f;

    private GameObject[] itemsOnTable;
    private bool itemPlaced = false; // Track if any item has been placed or not

    private void Start()
    {
        itemsOnTable = new GameObject[itemSlots.Length];

        // Initially set the health and timer UI to inactive
        if (healthUI != null) healthUI.SetActive(false);
        if (timerUI != null) timerUI.SetActive(false);
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
                    // Activate the timer and health UI on the first item placement
                    if (timerUI != null) timerUI.SetActive(true);
                    if (healthUI != null) healthUI.SetActive(true);

                    timer.StartTimer(); // Activate the timer script
                    //ChangeLightColor();  // Change the light color
                    StartCoroutine(ChangeLightColorOverTime());
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

     IEnumerator ChangeLightColorOverTime()
    {
        Color startColor = colorWhenItemPlaced;
        Color targetColor = colorAtEnd;
        float lastChangedTime = -10.0f;
        while (elapsedTime < colorChangeDuration)
        {
            // Calculate the new color based on the elapsed time.
            
            if(elapsedTime - lastChangedTime >= 10.0f){
                float t = elapsedTime / colorChangeDuration;
                // TODO: modify rate of change with a function
                directionalLight.color = Color.Lerp(startColor, targetColor, t);
                Debug.Log(directionalLight.color);
                Debug.Log(lastChangedTime);
                lastChangedTime = elapsedTime;
            }
           
            elapsedTime += Time.deltaTime;
            //Debug.Log(elapsedTime);
           
            yield return null; // Wait for the next frame.
        }

        // Ensure the color ends up as the target color.
        directionalLight.color = targetColor;
    }
}
