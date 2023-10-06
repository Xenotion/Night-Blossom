using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 5f;
    public float sprintSpeed = 8f;
    public float gravity = -20f;
    public float jumpHeight = 1.5f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool canMove = true;


    Vector3 velocity;
    bool isGrounded;
    float currentSpeed; // Variable to store the current movement speed

    private Vector3 knockbackTargetVelocity;
    private bool isBeingKnockedBack = false;

    private void Start()
    {
        currentSpeed = speed; // Initially set the current speed to the normal speed
    }

    void Update()
    {
        if (!canMove)
        {
            // Player cannot move, so don't process movement
            return;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (!isBeingKnockedBack) // Only allow player control when not being knocked back
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = sprintSpeed;
            }
            else
            {
                currentSpeed = speed;
            }

            controller.Move(move * currentSpeed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void SetMovementSpeed(float newSpeed)
    {
        currentSpeed = newSpeed;
    }

    public void ApplyKnockback(Vector3 direction, float force, float duration)
    {
        knockbackTargetVelocity = direction.normalized * force;
        if (isBeingKnockedBack)
        {
            StopCoroutine("PerformKnockback");
        }
        StartCoroutine(PerformKnockback(duration));
    }

    private IEnumerator PerformKnockback(float duration)
    {
        isBeingKnockedBack = true;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            controller.Move(knockbackTargetVelocity * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isBeingKnockedBack = false;
    }

}
