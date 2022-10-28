﻿using Photon.Pun;
using System.Collections;
using UnityEngine;

<<<<<<< HEAD
public class SoundManager : MonoBehaviour, IPunObservable
{
    public AudioClip[] footsteps;
    public AudioClip parkour;
    private AudioSource footstepSource;
    private AudioSource parkourSource;
    private PhotonView PV;
    private Rigidbody playerRigidbody;
    private PlayerMovement playerMovement;
    private int routineCount = 0;
    private IEnumerator routine, otherRoutine;
    private int current;
    public bool playParkourSound;
=======
public class SoundManager : MonoBehaviour//, IPunObservable
{
    public AudioClip[] footsteps;
    private AudioSource audioSource;
    private PhotonView PV;
    private Rigidbody playerRigidbody;
    private PlayerMovement playerMovement;
    private int routineCount;
    private IEnumerator routine;
    private int current;
>>>>>>> Jonathan

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        PV = GetComponent<PhotonView>();
        if (PV.IsMine) {
            var sources = GetComponents<AudioSource>();
            footstepSource = sources[0];
            //parkourSource = sources[1];
            //parkourSource.volume = 0.5f;
            playerRigidbody = GetComponent<Rigidbody>();
            playerMovement = GetComponent<PlayerMovement>();
        }
        
=======
        audioSource = GetComponent<AudioSource>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        PV = GetComponent<PhotonView>();
>>>>>>> Jonathan
        routineCount = 0;
        current = 0;
    }

<<<<<<< HEAD
    public void OnPhotonSerializeView(PhotonStream s, PhotonMessageInfo i) {
        if (s.IsWriting) {
            //Debug.Log("writing from soundmanager");
            s.SendNext(current);
            s.SendNext(routineCount);
            if (playerRigidbody != null)
                s.SendNext(playerRigidbody.velocity);
            if (playerMovement != null) {
                s.SendNext(playerMovement.isOnGround);
                s.SendNext(playerMovement.isWallrunning);
                s.SendNext(playerMovement.beforeWallJumpTimer);
                s.SendNext(playerMovement.playerState);
            }
        } else {
            //Debug.Log("reading in soundmanager");
            current = (int) s.ReceiveNext();
            routineCount = (int) s.ReceiveNext();
            if (playerRigidbody != null)
                playerRigidbody.velocity = (Vector3) s.ReceiveNext();
            if (playerMovement != null) {
                playerMovement.isOnGround = (bool) s.ReceiveNext();
                playerMovement.isWallrunning = (bool) s.ReceiveNext();
                playerMovement.beforeWallJumpTimer = (float) s.ReceiveNext();
                playerMovement.playerState = (PlayerMovement.MovementState) s.ReceiveNext();
            }
=======
    // public void OnPhotonSerializeView(PhotonStream s, PhotonMessageInfo i) {
    //     if (s.IsWriting) {
    //         Debug.Log("writing from soundmanager");
    //         s.SendNext(footsteps[current]);
    //     } else {
    //         Debug.Log("reading in soundmanager");
    //         //s.ReceiveNext
    //     }
    // }

    void Update() {
        if (PV.IsMine) {
            if (playerMovement.playerState == PlayerMovement.MovementState.walking
            || playerMovement.playerState == PlayerMovement.MovementState.sprinting) {
                if (routineCount == 0)
                {
                    routine = ChooseAFootstep();
                    StartCoroutine(routine);
                    routineCount++;   
                }
            } else {
                if (routineCount == 1 && routine != null) {
                    StopCoroutine(routine);
                    routineCount = 0;
                } 
            }
>>>>>>> Jonathan
        }
    }

    void Update() {
        if (PV.IsMine) {
            UpdateRoutine();
        } else { 
            UpdateRoutine();
        }
    }

    public void UpdateRoutine() {
        if (playerMovement.playerState == PlayerMovement.MovementState.walking
            || playerMovement.playerState == PlayerMovement.MovementState.sprinting) {
                if (routineCount == 0) {
                    routine = ChooseAFootstep();
                    StartCoroutine(routine);
                    routineCount++;   
                }
            } else {
                if (routineCount == 1 && routine != null) {
                    StopCoroutine(routine);
                    routineCount = 0;
                } 
            }
        // if (playParkourSound) {
        //     StartCoroutine(Parkour());
        // }
    }

    public IEnumerator Parkour() {
        parkourSource.PlayOneShot(parkour);
        playParkourSound = false;
        yield return new WaitForSeconds(1);
    }

    public IEnumerator ChooseAFootstep() {
        while(true) {
            if (current > 9) current = 0;
            float vel = playerRigidbody.velocity.magnitude;
            if ((playerMovement.playerState != PlayerMovement.MovementState.slowwalking && playerMovement.isOnGround && vel > 2f))
            {
<<<<<<< HEAD
                footstepSource.clip = footsteps[current];
                if (playerMovement.playerState == PlayerMovement.MovementState.walking) {
                    footstepSource.volume = 0.10f;
                    footstepSource.PlayOneShot(footstepSource.clip);

                    float interval = footstepSource.clip.length;
                    current++;
                    yield return new WaitForSeconds(interval + 0.20f);
                } else if (playerMovement.playerState == PlayerMovement.MovementState.sprinting) {
                    footstepSource.volume = 0.28f;
                    footstepSource.PlayOneShot(footstepSource.clip);

                    float interval = footstepSource.clip.length;
=======
                audioSource.clip = footsteps[current];
                if (playerMovement.playerState == PlayerMovement.MovementState.walking) {
                    audioSource.volume = 0.10f;
                    audioSource.PlayOneShot(audioSource.clip);

                    float interval = audioSource.clip.length;
                    current++;
                    yield return new WaitForSeconds(interval + 0.20f);
                } else if (playerMovement.playerState == PlayerMovement.MovementState.sprinting) {
                    audioSource.volume = 0.28f;
                    audioSource.PlayOneShot(audioSource.clip);

                    float interval = audioSource.clip.length;
>>>>>>> Jonathan
                    current++;
                    yield return new WaitForSeconds(interval);
                }
            }
            else
                yield return 0;
        }
    }
}
