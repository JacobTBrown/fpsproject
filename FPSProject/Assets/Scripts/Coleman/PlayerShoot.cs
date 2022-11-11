using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerShoot : MonoBehaviour
{
    public Action shootInput;
    public Action reloadInput;
    public Action pickupInput;
    public PlayerSettings keybinds;
    //[SerializeField] private KeyCode reloadKey;
    private PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        keybinds = GetComponent<PlayerSettings>();
    }

    private void Update()
    {
        if (PV.IsMine)
        {
            if (!DataManager.Instance.IsCanShoot) return;

            Gun gun = GetComponentInChildren<Gun>();
            if (gun != null)
            {
                if (!gun.gunData.fullAuto && gun.gameObject.activeSelf)
                {
                    if (Input.GetMouseButtonDown(0)) shootInput?.Invoke();
                }
                else
                {
                    if (Input.GetMouseButton(0)) shootInput?.Invoke();
                }
                if (Input.GetKeyDown(keybinds.inputSystemDic[KeycodeFunction.reload])) 
                {
                    reloadInput?.Invoke();
                }
                if (Input.GetKeyDown(KeyCode.E)) pickupInput?.Invoke();
            }
        }
    }
}
