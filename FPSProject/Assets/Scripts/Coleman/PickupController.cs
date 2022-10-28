using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    private PlayerMovement playerMove;
    private Gun self;

    private void Start()
    {
        self = GetComponent<Gun>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(WeaponPickup(other));
        }
    }

    IEnumerator WeaponPickup(Collider body)
    {
        GameObject player = body.gameObject.transform.parent.gameObject;
        playerMove = player.GetComponent<PlayerMovement>();
        GameObject weaponHolder = player.transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject gunObject = null;
        if (self.gunData.name == "AK-47") {
            gunObject = weaponHolder.transform.GetChild(2).gameObject;
            weaponHolder.GetComponent<WeaponSwap>().selected = 2;
            playerMove.PV.RPC("SetCurrentWeapon", RpcTarget.OthersBuffered, weaponHolder.GetComponent<WeaponSwap>().selected);
        }
        if (self.gunData.name == "SPAS-12") {
            gunObject = weaponHolder.transform.GetChild(1).gameObject;
            weaponHolder.GetComponent<WeaponSwap>().selected = 1;
            playerMove.PV.RPC("SetCurrentWeapon", RpcTarget.OthersBuffered, weaponHolder.GetComponent<WeaponSwap>().selected);
        }
        Gun gun = gunObject.GetComponent<Gun>();
        if (gun.owns == false)
        {
            gun.owns = true;
            weaponHolder.GetComponent<WeaponSwap>().selectWeapon();
            foreach (Transform child in self.transform)
            {
                if(child.GetComponent<MeshRenderer>() != null) child.GetComponent<MeshRenderer>().enabled = false;
            }
<<<<<<< HEAD
        } else canGrab = false;
    }

    [PunRPC]
    private void Pickup()
    {
        currentWeapon = weapon;
        currentWeapon.transform.parent = weaponHolder;
        currentWeapon.GetComponent<Gun>().WeaponHolder = weaponHolder.gameObject;
        currentWeapon.GetComponent<Gun>().PV = transform.gameObject.GetComponent<PhotonView>();
        currentWeapon.GetComponent<Gun>().enabled = true;
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
        currentWeapon.transform.localEulerAngles = new Vector3(0,180,0);
        currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
        weapon = null;
    }

    [PunRPC]
    private void Drop()
    {
        currentWeapon.transform.parent = droppedWeapons.transform;
        currentWeapon.transform.position = transform.position;
        currentWeapon.GetComponent<Gun>().PV = droppedWeapons.GetComponent<PhotonView>();
        currentWeapon.GetComponent<Gun>().enabled = false;
        currentWeapon.GetComponent<Rigidbody>().isKinematic = false;
        //currentWeapon.transform.position = weaponHolder.position;
        currentWeapon = null;
        weapon = null;
=======
            transform.position = new Vector3(0, 1000, 0);
            yield return new WaitForSeconds(1);
            GetComponent<Collider>().enabled = false;
            yield return new WaitForSeconds(3);
            Destroy(transform.gameObject);
        } else
        {
            if (gun.gunData.reserveAmmo < gun.gunData.maxReserveAmmo)
            {
                if(gun.gunData.reserveAmmo + gun.gunData.magSize > gun.gunData.maxReserveAmmo)
                {
                    gun.gunData.reserveAmmo = gun.gunData.maxReserveAmmo;
                } else
                {
                    gun.gunData.reserveAmmo += gun.gunData.magSize;
                }
                foreach (Transform child in self.transform)
                {
                    if (child.GetComponent<MeshRenderer>() != null) child.GetComponent<MeshRenderer>().enabled = false;
                }
                transform.position = new Vector3(0, 1000, 0);
                yield return new WaitForSeconds(1);
                GetComponent<Collider>().enabled = false;
                yield return new WaitForSeconds(3);
                Destroy(transform.gameObject);
            }
        }
>>>>>>> ZachNewHead
    }
}
