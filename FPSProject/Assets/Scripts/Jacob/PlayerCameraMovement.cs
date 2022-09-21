using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Author: Jacob Brown
    Creation: 9/20/22
    Last Edit: 9/21/22
*/
public class PlayerCameraMovement : MonoBehaviour
{
    [Header("Unity Classes")]
    public Transform cameraTransform;
    public Transform playerTransform;
    [Header("User Mouse Settings")]
    // Horizontal mouse sensitivity
    public float mouseXSensitivity = 250f;
    // Vertical mouse sensitivity
    public float mouseYSensitivity = 250f;
    // For the user to invert the Y mouse look
    public bool invertMouse = false;
    private float rotateYAxis, rotateXAxis;
    private Vector3 mouseMovement = new Vector3(0, 0, 0);
    private PlayerMovement playerMove;
    
    void Start()
    {
        playerMove = transform.parent.gameObject.GetComponent<PlayerMovement>();
        // Lock the cursor to the center of the screen 
        Cursor.lockState = CursorLockMode.Locked;
        // Make the cursor invisible
        Cursor.visible = false;
    }

    void Update()
    {
        GetInputs();
        InvertMouse(); 
        PerformRotation();
    }

    private void GetInputs() {
        // Get the horizontal mouse movement * sensitivity
        mouseMovement.x = Input.GetAxis("Mouse X") * mouseXSensitivity;
        // Get the vertical mouse movement * sensitivity
        mouseMovement.y = Input.GetAxis("Mouse Y") * mouseYSensitivity;
        mouseMovement *= Time.deltaTime;
    }

    private void InvertMouse() {
        // Setting for the user to invert the mouse when looking up and down
        if (invertMouse)
            rotateXAxis += mouseMovement.y;
        else
            rotateXAxis -= mouseMovement.y;
    }

    private void PerformRotation() {
        rotateYAxis += mouseMovement.x;
        // Clamp the mouse rotation X so that we can't look over and backwards.
        // By default the camera is looking forward at an x angle of 0, hence we use
        // -90f and 90f to prevent going too far down or too far up.
        rotateXAxis = Mathf.Clamp(rotateXAxis, -90f, 90f);
        cameraTransform.rotation = Quaternion.Euler(rotateXAxis, rotateYAxis, 0f);
        // If the player is on the ground. Then rotate the player on the y axis so that 
        // upon moving, we move relative to the player's orientation. This way
        // if we are in the air turning the camera doesn't alter the trajectory.
        if (playerMove.isOnGround)
            playerTransform.rotation = Quaternion.Euler(0f, rotateYAxis, 0f);
    }
}
