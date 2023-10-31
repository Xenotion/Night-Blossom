using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EndGame : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string paragraph; // A single string containing the entire paragraph
    public float textSpeed;
    private int index;

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == paragraph)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = paragraph;
            }
        }
    }

    void StartDialogue()
    {
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in paragraph.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        gameObject.SetActive(false); // If the entire paragraph is displayed, hide the text box.
    }
}
