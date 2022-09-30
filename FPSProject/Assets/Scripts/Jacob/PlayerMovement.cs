using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
/*
    Author: Jacob Brown
    Creation: 9/19/22
    Last Edit: 9/21/22

    This class handles movement in 3d space for a player character.
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
    public float doubleJumpTimer;
    public bool isOnGround;
    public bool canDoubleJump;
    
    private PlayerSettings keybinds;
    private PlayerCameraMovement playerCam;
    private float horizontalXInput;
    private float horizontalZInput;
    private Vector3 movement, velocity, limitedVelocity;

    public MovementState playerState;
    //Adding just a few lines below for PUN
    public PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    public enum MovementState {
        walking,
        sprinting,
        inAir,
    }

    void Start() {
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            //I'm not instantiating the gun at the moment. Logic for that will go here -zach
            
        }
        keybinds = GetComponent<PlayerSettings>();
        playerCam = GetComponentInChildren<PlayerCameraMovement>();
        
        // If we don't do this the player will fall over because it is a capsule
        playerRigidbody.freezeRotation = true;
    }
    
    void Update()
    {
        if (PV.IsMine)
        {  
            CheckForGround();
            GetInputs();
            UpdateState();
            ApplyDrag();
        }
      
        
    }

    void FixedUpdate() {
        if (PV.IsMine)
        {
            Move();
            // If the player presses the jump button and the player is grounded.
            if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.jump]) && isOnGround)
            {
                Jump();
            }
        }

    }

    private void CheckForGround() {
        /* This raycast simply points downwards from the player's position. It extends to half
        // the player's height + 0.1f. */
        isOnGround = Physics.Raycast(playerTransform.position, Vector3.down, 1.1f, groundLayer);
    }

    private void GetInputs() {
        if ((Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.leftMove]) 
            && Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.rightMove])) 
            || (!Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.leftMove]) 
            && !Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.rightMove])))
            horizontalXInput = 0;
        else if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.rightMove]))
            horizontalXInput = 1;
        else if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.leftMove]))
            horizontalXInput = -1;

        if ((Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.upMove])
            && Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.downMove]))
            || (!Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.upMove])
            && !Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.downMove])))
            horizontalZInput = 0;
        else if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.downMove]))
            horizontalZInput = -1;
        else if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.upMove]))
            horizontalZInput = 1;
    }

    public void UpdateState() {
        if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.sprint]) && isOnGround) {
            //IEnumerator coroutine = playerCam.AdjustFov(90);
            //StartCoroutine(coroutine);
            playerState = MovementState.sprinting;
            moveSpeed = groundSprintSpeed;
            canDoubleJump = true;
        } else if (isOnGround) {
            //IEnumerator coroutine = playerCam.AdjustFov(80);
            //StartCoroutine(coroutine);
            playerState = MovementState.walking;
            moveSpeed = groundWalkSpeed;
            canDoubleJump = true;
        } else {
            playerState = MovementState.inAir;
            moveSpeed = airSpeed;
        }
    }
    
    private void Jump() {
        // Reset the rigidbody y velocity to start all jumps at the same baseline velocity
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
        // Add an upwards impulse to the player rigidbody multiplied by the jumpForce
        playerRigidbody.AddForce(playerTransform.up * jumpForce, ForceMode.Impulse);

        // Let the player do a double jump after the specified amount of time.
        Invoke(nameof(DoubleJump), doubleJumpTimer);
    }

    private void DoubleJump() {
        /* If the player presses the jump button again and the player can double jump.*/
        if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.jump]) && canDoubleJump) {
            canDoubleJump = false;
            Jump();
        }
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

/* -- note from Zach 9/29 -- 
  For Multiplayer, I need an Instance of the player prefab --
  Currently, this is being created in the PlayerManager.cs file.
*/