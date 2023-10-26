using UnityEngine;
using TMPro;
using System.Collections;

public class ItemDetection : MonoBehaviour
{
    public ItemDialogue itemDialogue;
    private bool hasBeenDetected = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenDetected)
        {
            Debug.Log("Player entered trigger!");
            itemDialogue.setActive();
            hasBeenDetected = true; // Mark the detection as occurred
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger!");
        }
    }
}
