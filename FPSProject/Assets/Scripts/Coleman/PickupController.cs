using Photon.Pun;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public Transform weaponHolder;
    public float distance = 10;
    public GameObject currentWeapon;
    GameObject weapon;
    bool canGrab;

    private GameObject droppedWeapons;

    private PhotonView PV;
    private Animator anim;
    private BoxCollider _collider;

    void Start() {
        PV = GetComponent<PhotonView>();
        anim = currentWeapon.GetComponent<Animator>();
        _collider = currentWeapon.GetComponent<BoxCollider>();
        droppedWeapons = GameObject.Find("DroppedWeapons");
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
                    PV.RPC("Pickup", RpcTarget.OthersBuffered);
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
                    PV.RPC("Drop", RpcTarget.OthersBuffered);
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
                //Debug.Log("weapon detected");
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
