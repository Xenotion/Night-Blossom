using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;
    public GameObject gameOverText;
    public float knockbackForce = 20f;
    public float slowSpeed = 2.5f;
    public float slowDuration = 2f;

    // Camera shake variables
    public Transform cameraTransform;
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.1f;
    private Vector3 originalCameraPosition;

    private PlayerMovement playerMovement;
    private Coroutine slowCoroutine;

    // Mouse look script reference
    public MonoBehaviour mouseLookController;

    private void Start()
    {
        gameOverText.gameObject.SetActive(false);
        playerMovement = GetComponent<PlayerMovement>();
        
        if (cameraTransform != null)
        {
            originalCameraPosition = cameraTransform.localPosition;
        }
    }

    public void TakeDamage(int damageAmount, Vector3 attackerPosition)
    {
        Debug.Log("Player attacked");

        // Knockback effect
        Vector3 knockbackDirection = (transform.position - attackerPosition).normalized;
        playerMovement.ApplyKnockback(knockbackDirection, knockbackForce, 0.5f);

        // Apply slow effect
        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine);
        }
        playerMovement.SetMovementSpeed(slowSpeed);
        slowCoroutine = StartCoroutine(RestoreSpeedAfterDelay());

        // Camera shake
        if (cameraTransform != null)
        {
            StartCoroutine(CameraShake());
        }

        // Reduce health and check death
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator CameraShake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            cameraTransform.localPosition = new Vector3(x, y, originalCameraPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraTransform.localPosition = originalCameraPosition;
    }

    private IEnumerator RestoreSpeedAfterDelay()
    {
        yield return new WaitForSeconds(slowDuration);
        playerMovement.SetMovementSpeed(playerMovement.speed);
    }

    void Die()
    {
        gameOverText.gameObject.SetActive(true);

        // Restrict player movement and mouse look
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        if (mouseLookController != null)
        {
            mouseLookController.enabled = false;
        }

        // Make the cursor visible and unlock it
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Stop the scene from running
        Time.timeScale = 0f;
    }
}
