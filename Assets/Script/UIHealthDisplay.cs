using UnityEngine;
using UnityEngine.UI;

public class UIHealthDisplay : MonoBehaviour
{
    public PlayerHealth player; // Assign your player object with the health system in the inspector
    public Image[] hearts; // Assign your heart images in the inspector, make sure it's in order
    
    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < player.health)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
