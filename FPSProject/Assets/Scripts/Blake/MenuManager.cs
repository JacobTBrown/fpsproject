using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] public Menu[] menus;

    public static bool loadScene = false;
    private void Awake()
    {
        Instance = this;
        Debug.Log("MenuStart");
        //OpenMenu("welcome");
    }

    public void OpenMenu(string menuName)
    { 
        if(loadScene == true)
        {
            GameObject parent = GameObject.Find("Canvas");
            Transform[] childs = parent.GetComponentsInChildren<Transform>();
            //menuName = "TitleMenu";
        }
        for(int i = 0; i < menus.Length; i++)
        {
            if(menus[i].menuName == menuName)
            {
     
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
    //*good enough* scene transition logic
    //if we don't need to use the multiplayer api--Zach
    public void ChangeScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void quitApplication()
    {
        Application.Quit();
    }
}
