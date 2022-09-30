﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;

    [SerializeField] private KeyCode reloadKey;
    private Gun gun;

    private void Start()
    {

        
            PhotonView PV = GetComponent<PhotonView>();
        if (PV.IsMine)
            gun = GetComponentInChildren<Gun>();
        
        }
    private void Update()
    {

       // if (!gun.gunData.fullAuto && gun.gameObject.activeSelf)
        //{
            if (Input.GetMouseButtonDown(0)) shootInput?.Invoke();
        //} else
        //{
            if (Input.GetMouseButton(0)) shootInput?.Invoke();
        //}
        if (Input.GetKeyDown(reloadKey)) reloadInput?.Invoke();
    }
}
