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

        //GameObject titleMenu = GameObject.Find("TitleMenu");
        //titleMenu.SetActive(true);
    }
}
