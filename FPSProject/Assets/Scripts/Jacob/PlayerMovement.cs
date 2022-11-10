﻿using Photon.Pun;
using UnityEngine;
using System.Collections;
/*
    Author: Jacob Brown
    Creation: 9/19/22
    Last Edit: 9/21/22
    This class handles movement in 3d space for a player character.
*/
public class PlayerMovement : MonoBehaviour, IPunObservable
{
    [Header("Unity Classes")]
    public Rigidbody playerRigidbody;
    public Transform playerTransform;
    public Transform playerBodyTransform;
    public Transform cameraTransform;
    public Transform visorTransform;
    public LayerMask groundLayer;

    [Header("Movement Variables")]
    public float moveSpeed;
    // All values are set in the inspector
    public float groundDrag;
    public float slowWalkSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float powerUpSpeed;
    //wip
    public float wallrunSpeed;
    public float airDrag;
    public float airSpeed;
    public float crouchYScale;
    public float maxSlopeAngle;
    public float jumpForce;
    public float doubleJumpTimer;
    public bool isOnGround;
    public bool isOnSlope;
    public bool wallInFront;
    public bool wallToLeft;
    public bool wallToRight;
    public bool canDoubleJump;
    public bool canWallJump;
    public bool canStartSlide;
    public bool isClimbing;
    public bool isCrouching;
    public bool isSliding;
    public bool isWallrunning;
    public bool hasSpeedPowerup;
    public bool useGravity;
    public float antiGravForce;

    //timers
    public float maxSlideTime;
    public float slideTimer;
    public float maxBeforeWallJumpTime;
    public float beforeWallJumpTimer;
    //wip
    //public bool isWallrunning;
    public float wallCheckDistance;
    private PlayerSettings keybinds;
    private PlayerCameraMovement playerCam;
    private SoundManager playerSounds;
    private float horizontalXInput;
    private float horizontalZInput;
    private float initYScale;
    private Vector3 movement, velocity;
    private RaycastHit hitSlope, hitLeftWall, hitRightWall;

    public MovementState playerState;
    //Adding just a few lines below for PUN
    public PhotonView PV;
    
    public enum MovementState {
        slowwalking,
        walking,
        sprinting,
        crouching,
        climbing,
        sliding,
        wallrunning,
        inAir,
    }

    public void OnPhotonSerializeView(PhotonStream s, PhotonMessageInfo i) {
        if (s.IsWriting) {
            s.SendNext(moveSpeed);
            s.SendNext(walkSpeed);
            s.SendNext(airSpeed);
            s.SendNext(jumpForce);
            
            s.SendNext(isOnGround);
            s.SendNext(isOnSlope);
            s.SendNext(wallInFront);
            s.SendNext(wallToLeft);
            s.SendNext(wallToRight);
            s.SendNext(canWallJump);
            s.SendNext(canStartSlide);
            s.SendNext(isClimbing);
            s.SendNext(isCrouching);
            s.SendNext(isSliding);
            s.SendNext(isWallrunning);
            s.SendNext(hasSpeedPowerup);
            s.SendNext(useGravity);

            s.SendNext(playerState);
            s.SendNext(playerRigidbody.velocity);
        } else {
            moveSpeed = (float) s.ReceiveNext();
            walkSpeed = (float) s.ReceiveNext();
            airSpeed = (float) s.ReceiveNext();
            jumpForce = (float) s.ReceiveNext();

            isOnGround = (bool) s.ReceiveNext();
            isOnSlope = (bool) s.ReceiveNext();
            wallInFront = (bool) s.ReceiveNext();
            wallToLeft = (bool) s.ReceiveNext();
            wallToRight = (bool) s.ReceiveNext();
            canWallJump = (bool) s.ReceiveNext();
            canStartSlide = (bool) s.ReceiveNext();
            isClimbing = (bool) s.ReceiveNext();
            isCrouching = (bool) s.ReceiveNext();
            isSliding = (bool) s.ReceiveNext();
            isWallrunning = (bool) s.ReceiveNext();
            hasSpeedPowerup = (bool) s.ReceiveNext();
            useGravity = (bool) s.ReceiveNext();

            playerState = (PlayerMovement.MovementState) s.ReceiveNext();
            playerRigidbody.velocity = (Vector3) s.ReceiveNext();
        }
    }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start() {
        if (!PV.IsMine)
        {
            // Can only have one AudioListener in a scene
            GetComponentInChildren<AudioListener>().enabled = false;
            // Destroy the camera component so our guns don't disappear
            GetComponentInChildren<Camera>().enabled = false;
            visorTransform.gameObject.SetActive(true);
        } else {
            PhotonChatManager.instance.AddMoveSpeedEvent = () => { walkSpeed += 10f; };
            PhotonChatManager.instance.ReduceMoveSpeedEvent = () => { walkSpeed -= 2f; };
            PhotonChatManager.instance.AddJumpEvent = () => { jumpForce += 5f; };
            PhotonChatManager.instance.ReduceJumpEvent = () => { jumpForce -= 2f; };
            PhotonChatManager.instance.AddAirSpeedEvent = () => { airSpeed += 5f; };
            PhotonChatManager.instance.ReduceAirSpeedEvent = () => { airSpeed -= 2f; };
        }
        
        keybinds = GetComponent<PlayerSettings>();
        playerCam = GetComponentInChildren<PlayerCameraMovement>();
        playerSounds = GetComponent<SoundManager>();

        // If we don't do this the player will fall over because it is a capsule
        playerRigidbody.freezeRotation = true;
        initYScale = playerBodyTransform.localScale.y;
    }

    void Update()
    {
        if (PV.IsMine)
        {
            if (keybinds.chatIsOpen || keybinds.settingPanel.activeInHierarchy) return; //Added by Jacob to disable the player while chat is open
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
        // limit velocity if needed
        if (velXZ.magnitude > moveSpeed)
        {
            //print("velXZ mag: " + velXZ.magnitude);
            // Movement speed and jump speed are different values
            Vector3 newVelXZ = velXZ.normalized * moveSpeed;
            //Vector3 newVelY = velY.normalized * jumpForce;
            playerRigidbody.velocity = new Vector3(newVelXZ.x, playerRigidbody.velocity.y, newVelXZ.z);
        }
    }

    void FixedUpdate()
    {
        if (PV.IsMine)
        {
            if (playerState != MovementState.climbing) {
                if (isWallrunning && !isCrouching) {
                    WallrunMove();
                    if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.jump]) && !isOnGround && canWallJump)
                        WallJump();
                }
                else if (isSliding)
                    SlideMove();
                else {
                    Move();

                    // If the player presses the jump button and the player is grounded.
                    if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.jump]) && isOnGround)
                    {
                        Jump();
                        // Let the player do a double jump after the specified amount of time.
                        Invoke(nameof(DoubleJump), doubleJumpTimer);
                    }
                }    
            } else {
                playerRigidbody.useGravity = false;
                Jump();
            }
        }
    }

    private void WallrunMove() {
        playerRigidbody.useGravity = useGravity;

        Vector3 wallNormal = wallToRight ? hitRightWall.normal : hitLeftWall.normal;
        Vector3 wallUp = Vector3.Cross(-playerTransform.forward, wallNormal);
        Vector3 wallForward = Vector3.Cross(wallUp, wallNormal);

        playerRigidbody.AddForce(wallForward * moveSpeed, ForceMode.Force);
        
        if (!(wallToLeft && horizontalZInput > 0) && !(wallToRight && horizontalZInput > 0))
            playerRigidbody.AddForce(-wallNormal * 50, ForceMode.Force);
        
        if (useGravity)
            playerRigidbody.AddForce(transform.up * antiGravForce, ForceMode.Impulse);

        if (beforeWallJumpTimer > 0)
            beforeWallJumpTimer -= Time.fixedDeltaTime;
        else
            canWallJump = true;
    }

    Vector3 wallNormal;

    private void SlideMove() {
        if (isOnSlope) {
            playerRigidbody.AddForce(GetSlopeDirection(movement) * moveSpeed, ForceMode.Force);
        } else {
            // Moves our player based on the x-y-z of the normalized movement vector multiplied by targetSpeed
            playerRigidbody.AddForce(movement.normalized * moveSpeed, ForceMode.Force); 
        } 
        
        if (!isOnGround)
            playerRigidbody.AddForce(transform.up * -(antiGravForce * 1.5f), ForceMode.Impulse);

        if (!(wallToLeft && horizontalXInput > 0) && !(wallToRight && horizontalXInput < 0))
            playerRigidbody.AddForce(-wallNormal * 50, ForceMode.Force);
    }

    private IEnumerator displayRoutine;
    private int displayCount = 0;

    TextMesh nameMesh;
    private void RaycastChecks()
    {
        /* This raycast simply points downwards from the player's position. It extends to half
        // the player's height + 0.1f. */
        isOnGround = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
        wallInFront = Physics.Raycast(transform.position, playerTransform.forward, wallCheckDistance, groundLayer);
        wallToRight = Physics.Raycast(transform.position, playerTransform.right, out hitRightWall, wallCheckDistance, groundLayer);
        wallToLeft = Physics.Raycast(transform.position, -playerTransform.right, out hitLeftWall, wallCheckDistance, groundLayer);

        if (Physics.Raycast(transform.position, Vector3.down, out hitSlope, 1.3f))
        {
            float angle = Vector3.Angle(Vector3.up, hitSlope.normal);
            isOnSlope = angle < maxSlopeAngle && angle != 0;
            if (isOnSlope)
                isOnGround = true;
        }
        else
        {
            isOnSlope = false;
        }

        // When you look at another player this will make it so the name of the player rotates towards you
        if (Physics.Raycast(transform.position, cameraTransform.forward, out RaycastHit hitInfo, 100)) {
            if (hitInfo.collider.tag == "Player" && hitInfo.collider != this.playerBodyTransform.GetComponent<Collider>()) {
                if (nameMesh != null && nameMesh != hitInfo.collider.gameObject.GetComponentInParent<PlayerSettings>().playerName)
                    nameMesh.gameObject.SetActive(false);

                nameMesh = hitInfo.collider.gameObject.GetComponentInParent<PlayerSettings>().playerName;
                nameMesh.gameObject.SetActive(true);
                var lookPos = hitInfo.collider.gameObject.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                nameMesh.gameObject.transform.rotation = rotation;
            }
            else
            {
                if (nameMesh != null)
                    nameMesh.gameObject.SetActive(false);
                // if (displayCount == 1 && displayRoutine != null)
                // {
                //     displayCount = 0;
                //     StartCoroutine(displayRoutine);
                // }
            }
        }
    }

    public IEnumerator DisableName(TextMesh nameMesh) {
        yield return new WaitForSeconds(0.25f);
        nameMesh.gameObject.SetActive(false);
    }

    private void GetInputs()
    {
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
        if (Input.GetKeyUp(keybinds.inputSystemDic[KeycodeFunction.sprint])) {
            playerCam.AdjustFov(80f);
        }

        if (wallInFront && horizontalZInput > 0) {       // State - climbing
            playerState = MovementState.climbing;
            isClimbing = true;
        } else if (isWallrunning) { // State - Wallrunning
            playerState = MovementState.wallrunning;
            if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.sprint]))
                moveSpeed = sprintSpeed;
            else 
                moveSpeed = wallrunSpeed;
        } else if (isSliding) {
            playerState = MovementState.sliding;
        } else if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.crouch]) && isOnGround) {
            playerState = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        else if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.slowwalk]) && isOnGround)
        {
            playerState = MovementState.slowwalking;
            moveSpeed = slowWalkSpeed;
            canDoubleJump = true;
        }
        else if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.sprint]) && playerRigidbody.velocity.magnitude > 2f && isOnGround)
        {
            playerState = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            canDoubleJump = true;
            canStartSlide = true;
            playerCam.AdjustFov(100f);
        } else if (isOnGround) {
            playerState = MovementState.walking;
            moveSpeed = walkSpeed;
            canDoubleJump = true;
        }
        else
        {
            playerState = MovementState.inAir;
            moveSpeed = airSpeed;

            if (isClimbing)
            {
                playerRigidbody.useGravity = true;
                isClimbing = false;
            }
        }

        if (hasSpeedPowerup && !isSliding) {
            moveSpeed = powerUpSpeed;
        }

        if (Input.GetKeyDown(keybinds.inputSystemDic[KeycodeFunction.crouch])) {
            playerBodyTransform.localScale = new Vector3(playerBodyTransform.localScale.x, crouchYScale, playerBodyTransform.localScale.z);
            //transform.position = new Vector3(transform.position.x, transform.position.y - crouchYScale, transform.position.z);
            if (!isCrouching) isCrouching = true;
        } else if (Input.GetKeyUp(keybinds.inputSystemDic[KeycodeFunction.crouch])) {
            playerBodyTransform.localScale = new Vector3(playerBodyTransform.localScale.x, initYScale, playerBodyTransform.localScale.z);
            if (isCrouching) isCrouching = false;
        }

        if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.slide]) && playerRigidbody.velocity.magnitude > 2f && canStartSlide) {
            //playerBodyTransform.localScale = new Vector3(playerBodyTransform.localScale.x, crouchYScale, playerBodyTransform.localScale.z);
            //transform.position = new Vector3(transform.position.x, transform.position.y - crouchYScale, transform.position.z);
            if (!isSliding) {
                isSliding = true;
                canStartSlide = false;
                slideTimer = maxSlideTime;
                playerCam.AdjustFov(90f);
                playerCam.AdjustXTilt(-35f);
            }
        }

        if (isSliding && slideTimer > 0) {
            slideTimer -= Time.deltaTime;
        } else if (isSliding) {
            //playerBodyTransform.localScale = new Vector3(playerBodyTransform.localScale.x, initYScale, playerBodyTransform.localScale.z);
            isSliding = false;
            playerCam.AdjustFov(80f);
            playerCam.AdjustXTilt(0);
        }

        if ((wallToLeft || wallToRight) && horizontalZInput > 0 && !isOnGround) {
            if (!isWallrunning && !isCrouching && !isSliding) {
                playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
                isWallrunning = true;
                beforeWallJumpTimer = maxBeforeWallJumpTime;
                playerCam.AdjustFov(90f);
                if (wallToLeft) playerCam.AdjustZTilt(-15f);
                if (wallToRight) playerCam.AdjustZTilt(15f);
            }
            if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.sprint]))
                playerCam.AdjustFov(100f);
            else
                playerCam.AdjustFov(90f);
        } else {
            if (isWallrunning) {
                isWallrunning = false;
                canWallJump = false;
                playerCam.AdjustFov(80f);
                playerCam.AdjustZTilt(0);
            } 
        }
    }

    public float getTargetSpeed(MovementState state) {
        switch(state) {
            case MovementState.slowwalking:
                return slowWalkSpeed;
            case MovementState.walking:
                return walkSpeed;
            case MovementState.sprinting:
                return sprintSpeed;
            case MovementState.crouching:
                return crouchSpeed;
            case MovementState.inAir:
                return airSpeed;
            default:
                return moveSpeed;
        }
    }

    private void WallJump() {
        if (keybinds.chatIsOpen) return; //Added by zach to disable jump when chat is open

        //playerSounds.playParkourSound = true;

        Vector3 wallNormal = wallToRight ? hitRightWall.normal : hitLeftWall.normal;
        Vector3 force = transform.up * jumpForce + wallNormal * (0.50f * jumpForce);
        // Reset the rigidbody y velocity to start all jumps at the same baseline velocity
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
        // Add an upwards impulse to the player rigidbody multiplied by the jumpForce
        playerRigidbody.AddForce(force, ForceMode.Impulse);
    }
    
    private void Jump() {
        if (keybinds.chatIsOpen) return; //Added by zach to disable jump when chat is open
        // Reset the rigidbody y velocity to start all jumps at the same baseline velocity
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
        // Add an upwards impulse to the player rigidbody multiplied by the jumpForce
        playerRigidbody.AddForce(playerTransform.up * jumpForce, ForceMode.Impulse);
    }

    private void DoubleJump()
    {
        /* If the player presses the jump button again and the player can double jump.*/
        if (Input.GetKey(keybinds.inputSystemDic[KeycodeFunction.jump]) && canDoubleJump)
        {
            canDoubleJump = false;
            Jump();
        }
    }

    private void Move()
    {
        if (keybinds.chatIsOpen) return; //Added by zach to disable movement when chat is open
        // Set up our movement vector
        movement = playerTransform.right * horizontalXInput + playerTransform.forward * horizontalZInput;
            
        if (isOnSlope) {
            playerRigidbody.AddForce(GetSlopeDirection(movement) * moveSpeed, ForceMode.Force);
        } else {
            // Moves our player based on the x-y-z of the normalized movement vector multiplied by targetSpeed
            playerRigidbody.AddForce(movement.normalized * moveSpeed, ForceMode.Force); 
        }

        if (!isWallrunning) playerRigidbody.useGravity = !isOnSlope;
    }

    public Vector3 GetSlopeDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, hitSlope.normal).normalized;
    }

    private void ApplyDrag()
    {
        /* If the player is on the ground, then we want to prevent the player from moving forever
        // without any input.
        // So, we set drag to our groundDrag (Value set in inspector).
        // If the player is NOT on the ground, we set drag to airDrag. */
        if (isOnGround || isWallrunning || isSliding)
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

