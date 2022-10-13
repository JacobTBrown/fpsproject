using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public Transform weaponHolder;
    public float distance = 10;
    public GameObject currentWeapon;
    GameObject weapon;
    bool canGrab;
    void Update()
    {
        canGrab = false;
        CheckWeapons();
        if(canGrab)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if (currentWeapon != null) Drop();
                Pickup();
            }
        }
        if(currentWeapon != null)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                Drop();
            }
        }
    }

    private void CheckWeapons()
    {
        RaycastHit info;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out info, distance))
        {
            if(info.transform.tag == "Weapon")
            {
                canGrab = true;
                weapon = info.transform.gameObject;
            }
        } else canGrab = false;
    }

    private void Pickup()
    {
        currentWeapon = weapon;
        currentWeapon.transform.parent = weaponHolder;
        currentWeapon.GetComponent<Gun>().player = transform.gameObject;
        currentWeapon.GetComponent<Gun>().WeaponHolder = weaponHolder.gameObject;
        currentWeapon.GetComponent<Gun>().PV = transform.gameObject.GetComponent<PhotonView>();
        currentWeapon.GetComponent<Gun>().enabled = true;
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
        currentWeapon.transform.localEulerAngles = new Vector3(0,180,0);
        currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
        weapon = null;
    }

    private void Drop()
    {
        currentWeapon.transform.parent = null;
        currentWeapon.transform.position = weaponHolder.position;
        currentWeapon.GetComponent<Gun>().enabled = false;
        currentWeapon.GetComponent<Rigidbody>().isKinematic = false;
        currentWeapon.transform.position = weaponHolder.position;
        currentWeapon = null;
        weapon = null;
    }
}
