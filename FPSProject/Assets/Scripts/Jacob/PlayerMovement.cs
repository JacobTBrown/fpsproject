using Photon.Pun;
using UnityEngine;
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
    public float moveSpeed;
    // All values are set in the inspector
    public float groundDrag;
    public float groundSlowWalkSpeed;
    public float groundWalkSpeed;
    public float groundSprintSpeed;
    public float powerUpSpeed;
    //wip
    //public float wallrunSpeed;
    public float airDrag;
    public float airSpeed;
    public float jumpForce;
    public float doubleJumpTimer;
    public bool isOnGround;
    public bool wallInFront;
    public bool canDoubleJump;
    public bool isClimbing;
    public bool hasSpeedPowerup;
    //wip
    //public bool isWallrunning;
    public float wallCheckDistance;
    private PlayerSettings keybinds;
    private PlayerCameraMovement playerCam;
    private SoundManager playerSounds;
    private float horizontalXInput;
    private float horizontalZInput;
    private Vector3 movement, velocity;

    public MovementState playerState;
    //Adding just a few lines below for PUN
    public PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    public enum MovementState {
        slowwalking,
        walking,
        sprinting,
        climbing,
        //wip
        //wallrunning,
        inAir,
    }

    void Start() {
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<AudioListener>());
            Destroy(GetComponentInChildren<Camera>().gameObject);
            //wheres the other people's guns ?
        }

        keybinds = GetComponent<PlayerSettings>();
        playerCam = GetComponentInChildren<PlayerCameraMovement>();
        playerSounds = GetComponent<SoundManager>();
        
        // If we don't do this the player will fall over because it is a capsule
        playerRigidbody.freezeRotation = true;
    }
    
    void Update()
    {
        if (PV.IsMine)
        {  
            RaycastChecks();
            GetInputs();
            LimitSpeed();
            UpdateState();
            ApplyDrag();
        }
    }

    private void LimitSpeed()
    {
        // limiting x-z speed on ground or in air
        Vector3 velXZ = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
        // limiting y speed
        //Vector3 velY = new Vector3(0f, playerRigidbody.velocity.y, 0f);

        // limit velocity if needed
        if (velXZ.magnitude > moveSpeed)
        {
            // Movement speed and jump speed are different values
            Vector3 newVelXZ = velXZ.normalized * moveSpeed;
            //Vector3 newVelY = velY.normalized * jumpForce;
            playerRigidbody.velocity = new Vector3(newVelXZ.x, playerRigidbody.velocity.y, newVelXZ.z);
        }
    }

    void FixedUpdate() {
        if (PV.IsMine)
        {
            if (playerState != MovementState.climbing) {
                Move();
                // If the player presses the jump button and the player is grounded.
                if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.jump]) 
                    && (isOnGround)) //|| (wallrunning.wallLeft || wallrunning.wallRight)))
                {
                    Jump();
                    // Let the player do a double jump after the specified amount of time.
                    Invoke(nameof(DoubleJump), doubleJumpTimer);
                }
            } else {
                playerRigidbody.useGravity = false;
                Jump();
            }
        }
    }

    private void RaycastChecks() {
        /* This raycast simply points downwards from the player's position. It extends to half
        // the player's height + 0.1f. */
        isOnGround = Physics.Raycast(playerTransform.position, Vector3.down, 1.1f, groundLayer);
        wallInFront = Physics.Raycast(playerTransform.position, playerTransform.forward, wallCheckDistance, groundLayer);

        // When you look at another player this will make it so the name of the player rotates towards you
       /* if (Physics.Raycast(playerTransform.position, playerTransform.forward, out RaycastHit hitInfo, 100)) {
            if (hitInfo.collider.tag == "Player") {
                TextMesh nameMesh = hitInfo.collider.gameObject.transform.parent.GetComponentInChildren<TextMesh>();
                var lookPos = hitInfo.collider.gameObject.transform.position - playerTransform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                nameMesh.gameObject.transform.rotation = Quaternion.Slerp(nameMesh.gameObject.transform.rotation, rotation, Time.deltaTime * 2.5f);
            }
        }*/
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

        if ((Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.upMove])
            && Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.leftMove])))
        {
            horizontalZInput = 0.5f;
            horizontalXInput = -0.5f;
        }
        else if ((Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.upMove])
            && Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.rightMove])))
        {
            horizontalZInput = 0.5f;
            horizontalXInput = 0.5f;
        }

        if ((Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.downMove])
            && Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.leftMove])))
        {
            horizontalZInput = -0.5f;
            horizontalXInput = -0.5f;
        }
        else if ((Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.downMove])
            && Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.rightMove])))
        {
            horizontalZInput = -0.5f;
            horizontalXInput = 0.5f;
        }
    }

    public void UpdateState() {
        if (wallInFront && horizontalZInput > 0) {       // State - climbing
            playerState = MovementState.climbing;
            isClimbing = true;
        } /*else if (isWallrunning) { // State - Wallrunning
            playerState = MovementState.wallrunning;
            moveSpeed = wallrunSpeed;*/
        else if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.slowwalk]) && isOnGround) {
            playerState = MovementState.slowwalking;
            moveSpeed = groundSlowWalkSpeed;
            canDoubleJump = true;
        } else if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.sprint]) && isOnGround) {
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
            
            if (isClimbing) {
                playerRigidbody.useGravity = true;
                isClimbing = false;
            }
        }

        if (hasSpeedPowerup) {
            moveSpeed = powerUpSpeed;
        }
    }

    public float getMoveSpeed(MovementState state) {
        switch(state) {
            case MovementState.slowwalking:
                return groundSlowWalkSpeed;
            case MovementState.walking:
                return groundWalkSpeed;
            case MovementState.sprinting:
                return groundSprintSpeed;
            case MovementState.inAir:
                return airSpeed;
            default:
                return moveSpeed;
        }
    }
    
    private void Jump() {
        if (keybinds.chatIsOpen) return; //Added by zach to disable jump when chat is open
        // Reset the rigidbody y velocity to start all jumps at the same baseline velocity
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
        // Add an upwards impulse to the player rigidbody multiplied by the jumpForce
        playerRigidbody.AddForce(playerTransform.up * jumpForce, ForceMode.Impulse);
    }

    private void DoubleJump() {
        /* If the player presses the jump button again and the player can double jump.*/
        if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.jump]) && canDoubleJump) {
            canDoubleJump = false;
            Jump();
        }
    }

    private void Move() {
        if (keybinds.chatIsOpen) return; //Added by zach to disable movement when chat is open
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
    /*
    ##############################################################
    Requeest to Remove or Delete From Jonathan Alexander
    Reasons: This Code is not being used anywhere in the Project
             This Code should not be in player Movement
    ##############################################################
    public void TakeDamage(float damage)
    {
        Debug.Log("took damage : " + damage);
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!PV.IsMine)
        {
            Debug.Log("not your player!");
            return;
        }
        Debug.Log("RPC took damage! damage: " + damage);
    }
    */
}

/* -- note from Zach 9/29 -- 
  For Multiplayer, I need an instanceID of the player prefab --
  Currently, this is being created in the PlayerManager.cs file.
*/