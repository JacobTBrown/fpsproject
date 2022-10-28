﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

/*
    Author: Jacob Brown
    Creation: 9/20/22
    Last Edit: 9/21/22
    This class handles the camera attached to a player. It handles any and all
    functions related to manipulating the camera.
*/
public class PlayerCameraMovement : MonoBehaviour, IPunObservable
{
    [Header("Unity Classes")]
    public Transform cameraTransform;
    public Transform playerTransform;
    public Transform playerBodyTransform;
    public bool ifTilting;

    private float rotateYAxis, rotateXAxis;
    private Vector3 mouseMovement = new Vector3(0, 0, 0);
    private PlayerMovement playerMove;
    private PlayerSettings playerSettings;

    public void OnPhotonSerializeView(PhotonStream s, PhotonMessageInfo i) {
        if (s.IsWriting) {
            //Debug.Log("writing from soundmanager");
            s.SendNext(ifTilting);
            s.SendNext(rotateYAxis);
            s.SendNext(playerBodyTransform.rotation);
        } else {
            //Debug.Log("reading in soundmanager");
            ifTilting = (bool) s.ReceiveNext();
            rotateYAxis = (float) s.ReceiveNext();
            playerBodyTransform.rotation = (Quaternion) s.ReceiveNext();
        }
    }

    void Start()
    {
        playerMove = transform.parent.gameObject.GetComponent<PlayerMovement>();
        playerSettings = transform.parent.gameObject.GetComponent<PlayerSettings>();
        ifTilting = false;
        // Lock the cursor to the center of the screen 
        Cursor.lockState = CursorLockMode.Locked;
        //// Make the cursor invisible
        Cursor.visible = false;
    }

    void Update()
    {
        if (playerMove.PV)
        if (playerMove.PV.IsMine)
        {
            //if (EventSystem.current.IsPointerOverGameObject()) return;

            GetInputs();
            InvertMouse();
            PerformRotation();
        }
    }

    private void GetInputs() {
        // Get the horizontal mouse movement * sensitivity
        mouseMovement.x = Input.GetAxis("Mouse X") * playerSettings.mouseXSensitivity;
        // Get the vertical mouse movement * sensitivity
        mouseMovement.y = Input.GetAxis("Mouse Y") * playerSettings.mouseYSensitivity;
        mouseMovement *= Time.deltaTime;
    }

    private void InvertMouse() {
        // Setting for the user to invert the mouse when looking up and down
        if (playerSettings.invertMouse)
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
        playerTransform.rotation = Quaternion.Euler(0f, rotateYAxis, 0f);
        if (!ifTilting)
            playerBodyTransform.rotation = Quaternion.Euler(playerBodyTransform.rotation.eulerAngles.x, rotateYAxis, playerBodyTransform.rotation.eulerAngles.z);
    }

    public void AdjustFov(float value) {
        GetComponent<Camera>().DOFieldOfView(value, 0.25f);
    }

    public void AdjustZTilt(float value) {
        playerBodyTransform.DORotate(new Vector3(0, rotateYAxis, value), 0.25f);
        if (value > 0 || value < 0)
            ifTilting = true;
        else
            ifTilting = false;
    }

    public void AdjustXTilt(float value) {
        playerBodyTransform.DORotate(new Vector3(value, rotateYAxis, 0), 0.25f);
        if (value > 0 || value < 0)
            ifTilting = true;
        else
            ifTilting = false;
    }
}
