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

    [SerializeField] private KeyCode reloadKey;
    private PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        Gun gun = GetComponentInChildren<Gun>();
        if (PV.IsMine) {
            if (!DataManager.Instance.IsCanShoot) return;
<<<<<<< HEAD

            Gun gun = GetComponentInChildren<Gun>();
            if (!gun.gunData.fullAuto && gun.gameObject.activeSelf) 
            {
            //{
                if (Input.GetMouseButtonDown(0)) shootInput?.Invoke();
            //} else
            //{
                if (Input.GetMouseButton(0)) shootInput?.Invoke();
            //}
                if (Input.GetKeyDown(reloadKey)) reloadInput?.Invoke();
                if (Input.GetKeyDown(KeyCode.E)) pickupInput?.Invoke();
            }
=======
        if (!gun.gunData.fullAuto && gun.gameObject.activeSelf)
        {
            if (Input.GetMouseButtonDown(0)) shootInput?.Invoke();
        } else
        {
            if (Input.GetMouseButton(0)) shootInput?.Invoke();
        }
            if (Input.GetKeyDown(reloadKey)) reloadInput?.Invoke();
            if (Input.GetKeyDown(KeyCode.E)) pickupInput?.Invoke();
>>>>>>> dcb61ce94349c7b24a84bc23b9501d9c56864f51
        }
    }
}
