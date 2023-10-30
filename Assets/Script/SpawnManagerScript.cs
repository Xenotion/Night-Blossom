using UnityEngine;
using System.Collections.Generic;

public class SpawnManagerScript : MonoBehaviour
{
    public List<GameObject> itemsToSpawn; // The actual game objects you want to move
    public List<Transform> spawnPoints; // List of potential spawn points

    void Start()
    {
        RandomizeSpawnPoints();
        PlaceItems();
    }

    void RandomizeSpawnPoints()
    {
        // Shuffle the list of spawn points to randomize their order
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            Transform temp = spawnPoints[i];
            int randomIndex = Random.Range(i, spawnPoints.Count);
            spawnPoints[i] = spawnPoints[randomIndex];
            spawnPoints[randomIndex] = temp;
        }
    }

    void PlaceItems()
    {
        // Move each item to a random spawn point from the shuffled list
        for (int i = 0; i < itemsToSpawn.Count; i++)
        {
            if(spawnPoints.Count > i) // Check if there are enough spawn points
            {
                itemsToSpawn[i].SetActive(true); // Activate the item if it was initially deactivated
                itemsToSpawn[i].transform.position = spawnPoints[i].position; // Move the item to the spawn point
            }
        }
    }
}
