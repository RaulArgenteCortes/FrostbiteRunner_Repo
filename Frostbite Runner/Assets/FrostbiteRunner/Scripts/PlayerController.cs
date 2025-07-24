using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Stats")]
    public float acceleration;
    public float minSpeed;
    public float friction;
    [SerializeField] Vector2 moveInput;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private Vector3 playerSpeed;
    // Rotation:
    [SerializeField] float rotationSpeed;

    [Header("Ground Stats")]
    public bool isGrounded;

    [Header("Referencias")]
    [SerializeField] GameObject playerBody; // Player's visible part.
    [SerializeField] GameObject groundCheck;
    private Rigidbody playerRb;
    private Animator anim;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        groundCheck = GameObject.Find("GroundCheck");
        playerBody = GameObject.Find("Body");
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        Movement();

        Rotation();
    }

    void Movement()
    {
        // Translates the Vector2 inputs in a Vector3.
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        // Adds the force from the Vector3 to playerspeed and multiplies it with the acceleration.
        playerSpeed = moveDirection * acceleration;

        // Slows the player on their respective axis if there is no input on it.
        if (moveInput.x == 0)
        {
            if (playerRb.velocity.x > minSpeed || playerRb.velocity.x < -minSpeed)
            {
                playerRb.velocity = new Vector3(playerRb.velocity.x * friction, playerRb.velocity.y, playerRb.velocity.z);
            }
            else
            {
                playerRb.velocity = new Vector3(0, playerRb.velocity.y, playerRb.velocity.z);
            }
        }
        if (moveInput.y == 0)
        {
            if (playerRb.velocity.z > minSpeed || playerRb.velocity.z < -minSpeed)
            {
                playerRb.velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y, playerRb.velocity.z * friction);
            }
            else
            {
                playerRb.velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y, 0);
            }
        }

        // Aplies playerSpeed if the player is grounded
        if (isGrounded == true)
        {
            playerRb.AddForce(playerSpeed);
        }
    }

    void Rotation()
    {
        // Rotates the body depending on the input (if the player is on ground).
        if (moveDirection != Vector3.zero && isGrounded == true)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            playerBody.transform.rotation = Quaternion.RotateTowards(playerBody.transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void Dash()
    {
        

        
    }

    #region Input Methods

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (isGrounded == true)
        {
            //playerRb.velocity = new Vector3(0, playerRb.velocity.y, 0);

            Dash();
        }
    }

    #endregion
}