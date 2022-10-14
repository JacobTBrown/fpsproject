using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [ContextMenu("Exit")]
    public void goToTitleMenu()
    {
        //SceneManager.UnloadSceneAsync("ColemanWeaponsAndPowerups");
        SceneManager.LoadScene("InitialScene");
        MenuManager.loadScene = true;
        Debug.Log("clicked");
        //GameObject titleMenu = GameObject.Find("TitleMenu");
        //titleMenu.SetActive(true);
    }
    public void SaveAndDestroy()
    {
        GameObject roomManager = GameObject.Find("RoomManager");
        Destroy(roomManager); 
    }
}
