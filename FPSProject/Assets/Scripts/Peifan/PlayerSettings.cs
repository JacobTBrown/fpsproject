using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/*
    Author: Jacob Brown
    Creation: 9/19/22
    Last Edit: 9/21/22

    A class representing all of the actions that a player can take along 
    with the default key-mappings associated with said actions.
*/
public class PlayerSettings : MonoBehaviour
{
    //PlayerSettings Instance;
    //I think we need to create an instance of PlayerSettings for each player,
    //this way, each player prefab that we create will have their own settings

    public GameObject settingPanel;
    [Header("User Mouse Settings")]
    // Horizontal mouse sensitivity
    public float mouseXSensitivity = 500f;
    // Vertical mouse sensitivity
    public float mouseYSensitivity = 500f;

    public Slider mouseYSlider;
    public Slider mouseXSlider;
    // For the user to invert the Y mouse look
    public bool invertMouse = false;
    [Header("User Keybinds")]
    public Dictionary<KeycodeFunction, KeyCode> inputSystemDic = new Dictionary<KeycodeFunction, KeyCode>() {
        { KeycodeFunction.leftMove, KeyCode.A},
        { KeycodeFunction.rightMove, KeyCode.D},
        { KeycodeFunction.upMove, KeyCode.W},
        { KeycodeFunction.downMove, KeyCode.S},
        { KeycodeFunction.sprint, KeyCode.LeftShift},
        { KeycodeFunction.jump, KeyCode.Space},
        { KeycodeFunction.reload, KeyCode.R},
        { KeycodeFunction.scoreboard, KeyCode.Tab},
        { KeycodeFunction.menu, KeyCode.Escape},
        };


    // Update is called once per frame
    void Update()
    {
        mouseXSensitivity = mouseXSlider.value * 5;
        mouseYSensitivity = mouseYSlider.value * 5;
        mouseYSlider.transform.Find("tips").GetComponent<Text>().text =(int)mouseYSlider.value + "";
        mouseXSlider.transform.Find("tips").GetComponent<Text>().text =(int)mouseXSlider.value + "";
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("pressed esc");
            if (settingPanel.activeInHierarchy)
            {
                // Lock the cursor to the center of the screen 
                Cursor.lockState = CursorLockMode.Locked;
                //// Make the cursor invisible
                Cursor.visible = false;
                Debug.Log("panel was active");
            }
            else
            {
                Debug.Log("panel was inactive");
                settingPanel.SetActive(settingPanel.activeInHierarchy);
                // Lock the cursor to the center of the screen 
                Cursor.lockState = CursorLockMode.None;
                //// Make the cursor invisible
                //Cursor.visible = false;
            }
            settingPanel.SetActive(!settingPanel.activeInHierarchy);
        }
    }
   
    public bool SetKeycodeValue(KeycodeFunction keycodeFunction, KeyCode keyCode)
    {
        if (inputSystemDic.Values.Contains(keyCode))
        {
            Debug.Log(keyCode + "：Button logic already exists");
            return true;
        }
        else
        {
            inputSystemDic[keycodeFunction] = keyCode;
            return false;
        }
    }
}
public enum KeycodeFunction
{
    leftMove,
    rightMove,
    upMove,
    downMove,
    sprint,
    jump,
    reload,
    scoreboard,
    menu

}
