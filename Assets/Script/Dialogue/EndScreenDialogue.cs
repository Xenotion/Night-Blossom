using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EndGame : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public AudioClip typingSound; // Assign a typing sound effect in the inspector
    public AudioSource audioSource; // Assign your AudioSource in the inspector
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
                textComponent.text = paragraph; // Instantly display all text
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
            PlayTypingSound();
            if (Char.IsPunctuation(c)) yield return new WaitForSeconds(textSpeed * 10); // Pause for punctuation
            else yield return new WaitForSeconds(textSpeed);

            // Example of text animation or effect
            if (c == '!')
            {
                textComponent.fontSize += 10; // Make exclamation marks bigger
                yield return new WaitForSeconds(textSpeed * 2); // Pause for effect
                textComponent.fontSize -= 10;
            }
        }
    }

    void PlayTypingSound()
    {
        if (audioSource && typingSound)
        {
            audioSource.PlayOneShot(typingSound); // Play typing sound effect
        }
    }

    void NextLine()
    {
        // gameObject.SetActive(false); // If the entire paragraph is displayed, hide the text box.
    }
}
