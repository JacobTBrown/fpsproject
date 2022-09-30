using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
    Author: Jacob Brown
    Creation: 9/19/22
    Last Edit: 9/29/22 -Zach

    A class representing all of the actions that a player can take along 
    with the default key-mappings associated with said actions.
*/
public class PlayerSettings : MonoBehaviour
{
    PhotonView PV; //needed for the reference to the player prefab 
    PlayerManager playermanager; //to send data to the playerManager, which will instantate our playerPrefab.

    MenuManager menuManager;
    GameObject errorTextPopup;
    [SerializeField] TMP_Text errorText;
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

    void Start() {
        PV = GetComponent<PhotonView>();
        playermanager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();

        if (PV.IsMine)
        {
            settingPanel = GameObject.Find("SettingPanel");
            mouseYSlider = GameObject.FindGameObjectWithTag("SliderV").GetComponent<Slider>();
            mouseXSlider = GameObject.FindGameObjectWithTag("SliderH").GetComponent<Slider>();
            settingPanel.SetActive(false);
            errorTextPopup = GameObject.Find("ErrorTextPopup");
            errorText = errorTextPopup.GetComponent<TMP_Text>();
            errorTextPopup.SetActive(false);

        }
    }
    void Update()
    {
        if (PV)
        {
            if (!PV.IsMine)
            {
                return;
            }
        }
        mouseXSensitivity = mouseXSlider.value * 5;
        mouseYSensitivity = mouseYSlider.value * 5;
        mouseYSlider.transform.Find("tips").GetComponent<Text>().text =(int)mouseYSlider.value + "";
        mouseXSlider.transform.Find("tips").GetComponent<Text>().text =(int)mouseXSlider.value + "";
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            
            if (settingPanel.activeInHierarchy)
            {
                // Lock the cursor to the center of the screen 
                Cursor.lockState = CursorLockMode.Locked;
                //// Make the cursor invisible
                Cursor.visible = false;
            }
            else
            {
                settingPanel.SetActive(settingPanel.activeInHierarchy);
                // Lock the cursor to the center of the screen 
                Cursor.lockState = CursorLockMode.None;
                //// Make the cursor invisible
                Cursor.visible = true;
            }
            settingPanel.SetActive(!settingPanel.activeInHierarchy);
        }
    }
   
    public bool SetKeycodeValue(KeycodeFunction keycodeFunction, KeyCode keyCode)
    {
        if (inputSystemDic.Values.Contains(keyCode))
        {
            Debug.Log(keyCode + "：Button logic already exists");
            MenuManager.Instance.OpenMenu("error");
            errorText.text = "That key is already in use";
            return true;
        }
        else
        {
            inputSystemDic[keycodeFunction] = keyCode;
            return false;
        }
    }

    //Testing lines below for multiplayer - zach - 9-30
    void Die()
    {
        playermanager.KillPlayer();
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
