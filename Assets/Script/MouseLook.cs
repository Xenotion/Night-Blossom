using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 30f;
    public float leanAmount = 15f; // Amount to lean
    public float leanSpeed = 5f;  // Speed of the leaning transition

    public Transform playerBody;
    public Transform cameraPivot; // Camera's pivot point for leaning

    float xRotation = 0f;
    Quaternion targetLeanRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        targetLeanRotation = Quaternion.Euler(0, 0, 0);

        // Load mouse sensitivity from PlayerPrefs
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", mouseSensitivity);
    }


    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        // Determine target rotation based on input
        if (Input.GetKey(KeyCode.Q))
        {
            targetLeanRotation = Quaternion.Euler(0, 0, leanAmount);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            targetLeanRotation = Quaternion.Euler(0, 0, -leanAmount);
        }
        else
        {
            targetLeanRotation = Quaternion.Euler(0, 0, 0);
        }

        // Smoothly interpolate to the target rotation
        cameraPivot.localRotation = Quaternion.Slerp(cameraPivot.localRotation, targetLeanRotation, leanSpeed * Time.deltaTime);
    }
}

