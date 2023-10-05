using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int health = 3;
    public TMP_Text gameOverText;
    public float knockbackForce = 20f;
    public float slowSpeed = 2.5f;
    public float slowDuration = 2f;

    private PlayerMovement playerMovement;
    private Coroutine slowCoroutine;

    private void Start()
    {
        gameOverText.gameObject.SetActive(false);
        playerMovement = GetComponent<PlayerMovement>(); // Cache the PlayerMovement component reference
    }

    public void TakeDamage(int damageAmount, Vector3 attackerPosition)
    {
        Debug.Log("Player attacked");

        // Knockback effect
        Vector3 knockbackDirection = (transform.position - attackerPosition).normalized;
        playerMovement.ApplyKnockback(knockbackDirection, knockbackForce, 0.5f); // 0.5f is the knockback duration, adjust as needed

        // Apply slow effect
        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine);
        }
        playerMovement.SetMovementSpeed(slowSpeed);
        slowCoroutine = StartCoroutine(RestoreSpeedAfterDelay());

        // Reduce health and check death
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator RestoreSpeedAfterDelay()
    {
        yield return new WaitForSeconds(slowDuration);
        playerMovement.SetMovementSpeed(playerMovement.speed); // Reset to the normal speed stored in PlayerMovement
    }

    void Die()
    {
        gameOverText.gameObject.SetActive(true);
    }
}
