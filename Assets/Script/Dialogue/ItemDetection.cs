using UnityEngine;
using TMPro;
using System.Collections;

public class ItemDetection : MonoBehaviour
{
    public ItemDialogue itemDialogue;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger!");
            itemDialogue.setActive();
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