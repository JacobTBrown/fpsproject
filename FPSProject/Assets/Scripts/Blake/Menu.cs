using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string menuName;
    public bool open;
    public void Open()
    {
        //Debug.Log("Menu script called Open()");
        open = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        Debug.Log("Menu script called Close()");
        open = false;
        gameObject.SetActive(false);
    }
}
