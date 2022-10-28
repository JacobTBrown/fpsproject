using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponSwap : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement player;
    public int selected;
    private int previous;
    // Start is called before the first frame update
    void Start()
    {
        selectWeapon();

        if (!player.PV.IsMine)
        previous = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.PV.IsMine) {
            int previous = selected;
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (selected >= transform.childCount - 1) selected = 0;
                else selected++;
                player.PV.RPC("SetCurrentWeapon", RpcTarget.OthersBuffered, selected);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (selected <= 0) selected = transform.childCount-1;
                else selected--;
                player.PV.RPC("SetCurrentWeapon", RpcTarget.OthersBuffered, selected);
            }
            if(previous != selected)
            {
                selectWeapon();
            }
        } else {
            if(previous != selected)
            {
                selectWeapon();
            }
            previous = selected;
        }
    }

    public void selectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if (i == selected && weapon.gameObject.GetComponent<Gun>().owns == true)
            {
                weapon.gameObject.SetActive(true);
                weapon.gameObject.GetComponent<Gun>().equipped = true;
                weapon.gameObject.GetComponent<Gun>().owns = true;
            }
            else
            {
                weapon.gameObject.GetComponent<Gun>().equipped = false;
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
