using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Author: Jacob Brown
    Creation: 9/19/22
    Last Edit: 9/20/22
*/
public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform playerTransform;
    public Vector3 velocity = new Vector3(0, 0, 0);
    public Vector3 gravity = new Vector3(0, -9.81f, 0);
    private Vector3 movement;

    // Movement speed along x-z axis
    public float movementSpeed = 4.0f;
    public float jumpHeight = 5.0f;
    // Update is called once per frame
    void Update()
    {
        // Using GetAxis insures that if we want to use a controller no extra work will 
        // be necessary, this function ranges from 1 to -1.
        velocity.x = Input.GetAxis("Horizontal");
        velocity.z = Input.GetAxis("Vertical");

        Jump();

        // Set up our movement vector
        movement = playerTransform.right * velocity.x + playerTransform.forward * velocity.z;
        // Multiply by our move speed and Time.deltaTime for smooth movement
        movement *= movementSpeed * Time.deltaTime;
        // Add the vertical axis as well for jumping
        movement += playerTransform.up * velocity.y * Time.deltaTime;
        // Moves our player based on the x-y-z of the movement vector
        controller.Move(movement);
    }

    private void Jump() {
        // If the player is mid-air
        if (!controller.isGrounded) {
            // Add gravity.y * Time.deltaTime simulating gravity
            velocity.y += gravity.y * Time.deltaTime;
        }
        // If the player presses the jump button and the player is on the ground
        if (Input.GetButton("Jump") && controller.isGrounded) {
            // Set the vertical velocity to jumpHeight * -gravity.t clamped by the jumpHeight
            velocity.y = Mathf.Clamp(jumpHeight * -gravity.y, -1, jumpHeight);
        }
        // If the player is on the ground and the vertical velocity is negative (gravity being applied)
        if (controller.isGrounded && velocity.y < 0) {
            // Reset the velocity y to a nice number
            velocity.y = -1f;
        }
    }
}