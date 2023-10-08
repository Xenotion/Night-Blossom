using UnityEngine;
using TMPro;
using System.Collections;

public class ItemDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;
    private bool dialogueStarted = false;

    void Start()
    {
        gameObject.SetActive(false);
        textComponent.text = string.Empty;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && dialogueStarted)
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            // Should remove the dialogue box at end of text..?
            gameObject.SetActive(false);
        }
    }

    public void setActive()
    {
        gameObject.SetActive(true);
        dialogueStarted = true;
        StartDialogue();
    }

}