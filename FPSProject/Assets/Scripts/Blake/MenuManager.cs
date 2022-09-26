using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public GameObject settingMenu;
    public bool isPreseed;
    [SerializeField] Menu[] menus;
    

    private void Awake()
    {
        Instance = this;
    }

    public void OpenMenu(string menuName)
    { 
        for(int i = 0; i < menus.Length; i++)
        {
            if(menus[i].menuName == menuName)
            {
                Debug.Log("Menu script called Open()");
                menus[i].Open();
            }
            else if(menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }

        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    //Adding a function below for Scene Transition logic - Zach
    public void ChangeScene(int secneID)
    {
        SceneManager.LoadScene(secneID);
    }

    private void Start()
    {
        settingMenu.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPreseed)
            {
                resume();
            }
            else
            {
                pause();
            }
        }
    }

    public void resume()
    {
        settingMenu.SetActive(true);
        isPreseed = false;
        Cursor.lockState = CursorLockMode.None;
        
    }
    public void pause() {
        settingMenu.SetActive(false);
        isPreseed = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
