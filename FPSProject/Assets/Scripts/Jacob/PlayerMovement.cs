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
    // All values are set in the inspector
    public float groundDrag;
    public float groundSpeed;
    public float airDrag;
    public float airSpeed;
    public float jumpForce;
    public float jumpTimer;
    public bool isOnGround;
    public bool canJump;
    
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
        
        // Moves our player based on the x-y-z of the movement vector
        if (isOnGround) {
            // If we are on the ground we want to use our groundSpeed
            playerRigidbody.AddForce(movement.normalized * groundSpeed, ForceMode.Force);  
        } else {
            /* If we are in the air we want to be able to move just a little bit but not 
            // like we are still grounded. So, airSpeed should typically be less than half
            // of groundSpeed. */
            playerRigidbody.AddForce(movement.normalized * airSpeed, ForceMode.Force);
        }   

        LimitMovementSpeed();
    }

    private void LimitMovementSpeed() {
        /* A temporary velocity variable, we are only concerned with x-z movement. */
        velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);

        /* This will ensure our player cannot accelerate infinitely and is capped to our
        // groundSpeed variable. */
        if (velocity.magnitude > groundSpeed) {
            limitedVelocity = velocity.normalized * groundSpeed;
            playerRigidbody.velocity = new Vector3(limitedVelocity.x, playerRigidbody.velocity.y, limitedVelocity.z);
        }
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