using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    public int selected;
    // Start is called before the first frame update
    void Start()
    {
        selectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int previous = selected;
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selected >= transform.childCount - 1) selected = 0;
            else selected++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (selected <= 0) selected = transform.childCount-1;
            else selected--;
        }
        if(previous != selected)
        {
            selectWeapon();
        }
    }

    void selectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if (i == selected)
            {
                weapon.gameObject.SetActive(true);
                weapon.gameObject.GetComponent<Gun>().equipped = true;
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
