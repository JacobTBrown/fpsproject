using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Author: Jacob Brown
    Creation: 9/19/22
    Last Edit: 9/21/22
*/
public class PlayerMovement : MonoBehaviour
{
    [Header("Unity Classes")]
    public Rigidbody playerRigidbody;
    public Transform playerTransform;
    public LayerMask groundLayer;
    
    [Header("Movement Variables")]
    private float moveSpeed;
    // All values are set in the inspector
    public float groundDrag;
    public float groundWalkSpeed;
    public float groundSprintSpeed;
    public float airDrag;
    public float airSpeed;
    public float jumpForce;
    public float jumpTimer;
    public bool isOnGround;
    public bool canJump;
    public bool canDoubleJump;
    
    private float horizontalXInput;
    private float horizontalZInput;
    private Vector3 movement, velocity, limitedVelocity;

    void Start() {
        // If we don't do this the player will fall over because it is a capsule
        playerRigidbody.freezeRotation = true;
    }
    
    void Update()
    {
        CheckForGround();
        GetInputs();
        ApplyDrag();
    }

    void FixedUpdate() {
        Move();
    }

    private void CheckForGround() {
        /* This raycast simply points downwards from the player's position. It extends to half
        // the player's height + 0.1f. */
        isOnGround = Physics.Raycast(playerTransform.position, Vector3.down, 1.1f, groundLayer);
    }

    private void GetInputs() {
        /* This function ranges from 1 to -1. */
        horizontalXInput = Input.GetAxis("Horizontal");
        horizontalZInput = Input.GetAxis("Vertical");
        
        /* If the player presses the jump button, the player is grounded, and can jump are all true.*/
        if (Input.GetButton("Jump") && canJump && isOnGround) {
            Jump();
        }

        if (Input.GetButton("Sprint") && isOnGround)
            moveSpeed = groundSprintSpeed;
        else if (isOnGround)
            moveSpeed = groundWalkSpeed;
        else
            moveSpeed = airSpeed;  
    }

    private void StopPlayer() {
        playerRigidbody.velocity = Vector3.zero;
    }
    
    private void Jump() {
        // Then set canJump = false, perform the jump, and use Invoke to call SetCanJumpTrue
        // after a set amount of time i.e. jumpTimer. */
        canJump = false;
        
        // Reset the rigidbody y velocity to start all jumps at the same baseline
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
        // Add an upwards impulse to the player rigidbody multiplied by the jumpForce
        playerRigidbody.AddForce(playerTransform.up * jumpForce, ForceMode.Impulse);

        /* Without resetting on a timer, the jumps can be inconsistent. Most times
        // a jump is executed as expected, but sometimes you will get an extra large jump. */
        Invoke(nameof(SetCanJumpTrue), jumpTimer);
    }

    private void SetCanJumpTrue() {
        canJump = true;
    }

    private void Move() {
        // Set up our movement vector
        movement = playerTransform.right * horizontalXInput + playerTransform.forward * horizontalZInput;
            
        // Moves our player based on the x-y-z of the normalized movement vector multiplied by moveSpeed
        playerRigidbody.AddForce(movement.normalized * moveSpeed, ForceMode.Force); 
    }
    
    private void ApplyDrag() {
        /* If the player is on the ground, then we want to prevent the player from moving forever
        // without any input.
        // So, we set drag to our groundDrag (Value set in inspector).

        // If the player is NOT on the ground, we set drag to airDrag. */
        if (isOnGround)
            playerRigidbody.drag = groundDrag;
        else
            playerRigidbody.drag = airDrag;
    }
}