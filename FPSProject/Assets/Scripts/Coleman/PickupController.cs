using Photon.Pun;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public Transform weaponHolder;
    public float distance = 10;
    public GameObject currentWeapon;
    GameObject weapon;
    bool canGrab;

<<<<<<< HEAD
    private GameObject droppedWeapons;

=======
>>>>>>> Jonathan
    private PhotonView PV;
    private Animator anim;
    private BoxCollider _collider;

    void Start() {
        PV = GetComponent<PhotonView>();
        anim = currentWeapon.GetComponent<Animator>();
        _collider = currentWeapon.GetComponent<BoxCollider>();
<<<<<<< HEAD
        droppedWeapons = GameObject.Find("DroppedWeapons");
=======
>>>>>>> Jonathan
    }

    void Update()
    {
        if (PV.IsMine) { 
            canGrab = false;
            CheckWeapons();
            if(canGrab)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    if (currentWeapon != null) Drop();
                    anim.enabled = true;
                    _collider.isTrigger = true;
                    Pickup();
<<<<<<< HEAD
                    PV.RPC("Pickup", RpcTarget.OthersBuffered);
=======
>>>>>>> Jonathan
                }
            }
            if(currentWeapon != null)
            {
                if(Input.GetKeyDown(KeyCode.Q))
                {
                    anim = currentWeapon.GetComponent<Animator>();
                    _collider = currentWeapon.GetComponent<BoxCollider>();
                    anim.enabled = false;
                    _collider.isTrigger = false;
                    Drop();
<<<<<<< HEAD
                    PV.RPC("Drop", RpcTarget.OthersBuffered);
=======
>>>>>>> Jonathan
                }
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
<<<<<<< HEAD
                //Debug.Log("weapon detected");
=======
                Debug.Log("weapon detected");
>>>>>>> Jonathan
                canGrab = true;
                weapon = info.transform.gameObject;
            }
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
    }
}
