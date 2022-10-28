using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerShoot : MonoBehaviour
{
    public Action shootInput;
    public Action reloadInput;
    public static Action pickupInput;

    [SerializeField] private KeyCode reloadKey;
    private PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

<<<<<<< HEAD
=======
        
            PhotonView PV = GetComponent<PhotonView>();
        if (PV.IsMine && GetComponentInChildren<Gun>()) {
            Debug.Log(GetComponentInChildren<Gun>().name + " was the name");

            gun = GetComponentInChildren<Gun>();
        }
        
        }
>>>>>>> Blake_Brooks(Current)
    private void Update()
    {
        if (PV.IsMine) {
            if (!DataManager.Instance.IsCanShoot) return;
       // if (!gun.gunData.fullAuto && gun.gameObject.activeSelf)
        //{
            if (Input.GetMouseButtonDown(0)) shootInput?.Invoke();
        //} else
        //{
            if (Input.GetMouseButton(0)) shootInput?.Invoke();
        //}
            if (Input.GetKeyDown(reloadKey)) reloadInput?.Invoke();
            if (Input.GetKeyDown(KeyCode.E)) pickupInput?.Invoke();
        }
    }
}
