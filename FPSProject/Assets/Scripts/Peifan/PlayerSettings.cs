﻿using Photon.Pun;
using Photon.Realtime;
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
    ErrorTextFade errorText; //setting errorText in start with GameObject.Find("errorTextPopup"); -zach
    public int instanceID;
    public int viewID;
    public string nickname;
    public GameObject settingPanel;
    public GameObject canvas;
    public GameObject weaponHolder;
    public GameObject scoreBoard;
    public TextMesh playerName;
    public GameObject chatRoom;
    [Header("User Mouse Settings")]
    // Horizontal mouse sensitivity
    public float mouseXSensitivity = 500f;
    // Vertical mouse sensitivity
    public float mouseYSensitivity = 500f;
    Vector3 timerPosition;
    Vector3 FFAPanelPosition;
    Vector3 TDMPanelPosition;
    public Slider mouseYSlider;
    public Slider mouseXSlider;
    private string ySliderText;
    private string xSliderText;
    // For the user to invert the Y mouse look
    public bool invertMouse = false;
    public bool chatIsOpen = false;
    public List<int> team;
    Vector3 hideUIOffScreenVector = new Vector3(0, -2500, 0);
    [Header("User Keybinds")]
    public Dictionary<KeycodeFunction, KeyCode> inputSystemDic = new Dictionary<KeycodeFunction, KeyCode>() {
        { KeycodeFunction.leftMove, KeyCode.A},
        { KeycodeFunction.rightMove, KeyCode.D},
        { KeycodeFunction.upMove, KeyCode.W},
        { KeycodeFunction.downMove, KeyCode.S},
        { KeycodeFunction.slowwalk, KeyCode.LeftControl},
        { KeycodeFunction.sprint, KeyCode.LeftShift},
        { KeycodeFunction.crouch, KeyCode.C},
        { KeycodeFunction.slide, KeyCode.F},
        { KeycodeFunction.jump, KeyCode.Space},
        { KeycodeFunction.reload, KeyCode.R},
        { KeycodeFunction.scoreboard, KeyCode.Tab},
        { KeycodeFunction.menu, KeyCode.Escape},
        { KeycodeFunction.chatRoom, KeyCode.KeypadEnter},
        };

    void Start() {
      
        Invoke("SetTeams", 0.5f);
        PV = GetComponent<PhotonView>();
        canvas = GameObject.FindGameObjectWithTag("Settings");
        weaponHolder = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).gameObject;
        
        // Added by Jacob Brown: 10/03/2022
        // created some variables for correctly identifying players 
        instanceID = this.gameObject.GetInstanceID();
        viewID = PV.ViewID;
        nickname = PV.Owner.NickName;
        // end add by Jacob Brown
    
        if (PV.IsMine)
        {
            
            // Added by Jacob Brown: 10/13/2022
            playerName = GetComponentInChildren<TextMesh>();
            playerName.text = nickname;
            playerName.gameObject.SetActive(false);
            // end add by Jacob Brown
            settingPanel = GameObject.Find("SettingPanel");
            //Debug.Log("GETTING SETTINGS PANEL");
            scoreBoard = GameObject.FindObjectOfType<Scoreboard>().gameObject; //adding logic for team vs ffa scoreboard - zach
            scoreBoard.transform.localPosition = hideUIOffScreenVector;
            GameObject scoreBoardTeams = GameObject.FindObjectOfType<ScoreboardTeams>().gameObject;
            scoreBoardTeams.transform.localPosition = hideUIOffScreenVector;
           // timerPosition = new Vector3(-558, -26, 0); // magic numbers: correspond to the initial position of the game objects on the canvas
            timerPosition = GameObject.Find("Timer").transform.localPosition; // magic numbers: correspond to the initial position of the game objects on the canvas
            
            FFAPanelPosition = GameObject.Find("Free For All Panel").transform.localPosition;  //is the default number as shown in the inspector.
            TDMPanelPosition = GameObject.Find("ScoreboardTeams").transform.localPosition;  //if you move it in the editor, you need to change these!
            errorText = GameObject.Find("ErrorTextPopup").GetComponent<ErrorTextFade>();
            if ((int)PhotonNetwork.LocalPlayer.CustomProperties["team"] > 0)
            {
                scoreBoard.SetActive(false);
                //Debug.Log("Set team scoreboard");
                scoreBoard = scoreBoardTeams;
            }
            else scoreBoardTeams.SetActive(false); //end - zach

            chatRoom = GameObject.Find("ChatPannel");
            chatRoom.SetActive(false);
            mouseYSlider = GameObject.FindGameObjectWithTag("SliderV").GetComponent<Slider>();
            mouseXSlider = GameObject.FindGameObjectWithTag("SliderH").GetComponent<Slider>();
            ySliderText = mouseYSlider.transform.Find("Slider").Find("tips").GetComponent<Text>().text = (int) mouseYSlider.value + "";
            xSliderText = mouseXSlider.transform.Find("Slider").Find("tips").GetComponent<Text>().text = (int) mouseXSlider.value + "";
            settingPanel.SetActive(false);
            
        } else {
            playerName = GetComponentInChildren<TextMesh>();
            playerName.text = nickname;
            playerName.gameObject.SetActive(false);
        }

        //debugs if you need them - Jacob B
        //Debug.Log("Instance ID: " + instanceID);
        //Debug.Log("View ID: " + viewID);
        //Debug.Log("Nickname: " + nickname);
    }
    void Update()
    {
        if (PV)
        {
            // Code added - Jacob Brown 10/13/2022
            // Reassigns the nickname whenever it is updated
            // Also transforms the playerName
            if (nickname != PV.Owner.NickName) {
                nickname = PV.Owner.NickName;
                playerName.text = nickname;
                PV.RPC("UpdatePlayerName", RpcTarget.OthersBuffered, nickname);
            }

            if (!PV.IsMine)
            {
                return;
            }
        }
        mouseXSensitivity = mouseXSlider.value * 5;
        mouseYSensitivity = mouseYSlider.value * 5;
        
        if (settingPanel.activeSelf) {
            ySliderText = mouseYSlider.transform.Find("Slider").Find("tips").GetComponent<Text>().text =(int)mouseYSlider.value + "";
            xSliderText = mouseXSlider.transform.Find("Slider").Find("tips").GetComponent<Text>().text =(int)mouseXSlider.value + "";
        }
        
        if (Input.GetKeyUp(inputSystemDic[KeycodeFunction.scoreboard]))
        {
            //if (scoreBoard.activeInHierarchy)
            
           if (scoreBoard.transform.localPosition != hideUIOffScreenVector)
            {
                GameObject.FindObjectOfType<Timer>().transform.localPosition = timerPosition;
                GameObject.FindObjectOfType<FFAPlayerScoreText>().transform.localPosition = FFAPanelPosition;
                scoreBoard.transform.localPosition = hideUIOffScreenVector;
            } else
            {
                GameObject.FindObjectOfType<Timer>().transform.localPosition = hideUIOffScreenVector;
                GameObject.FindObjectOfType<FFAPlayerScoreText>().transform.localPosition = hideUIOffScreenVector;
                scoreBoard.transform.localPosition = new Vector3(0, 0,0);
            }
        }

        if (Input.GetKeyUp(inputSystemDic[KeycodeFunction.chatRoom]))
        {
            if (chatRoom.activeInHierarchy)
            {
                chatRoom.SetActive(false);
                chatIsOpen = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                chatRoom.SetActive(true);
                chatIsOpen = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        int selected = weaponHolder.GetComponent<WeaponSwap>().selected;
        Gun gun = weaponHolder.transform.GetChild(selected).GetComponent<Gun>();
        if (Input.GetKeyUp(inputSystemDic[KeycodeFunction.menu]) && !gun.gunData.isReloading)
        {
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            gameObject.GetComponent<PlayerMovement>().moveSpeed = 0;        
            if (settingPanel.activeInHierarchy)
            {
                canvas.transform.GetChild(1).gameObject.SetActive(true);
                canvas.transform.GetChild(2).gameObject.SetActive(true);
                canvas.transform.GetChild(3).gameObject.SetActive(true);
                canvas.transform.GetChild(4).gameObject.SetActive(true);
                weaponHolder.SetActive(true);
                // Lock the cursor to the center of the screen 
                Cursor.lockState = CursorLockMode.Locked;
                //// Make the cursor invisible
                Cursor.visible = false;
            }
            else
            {
                canvas.transform.GetChild(1).gameObject.SetActive(false);
                canvas.transform.GetChild(2).gameObject.SetActive(false);
                canvas.transform.GetChild(3).gameObject.SetActive(false);
                canvas.transform.GetChild(4).gameObject.SetActive(false);
                weaponHolder.SetActive(false);
                // Lock the cursor to the center of the screen 
                Cursor.lockState = CursorLockMode.None;
                //// Make the cursor invisible
                Cursor.visible = true;
            }

            settingPanel.SetActive(!settingPanel.activeInHierarchy);
        } else if (Input.GetKeyUp(inputSystemDic[KeycodeFunction.menu]) && gun.gunData.isReloading) {
            Cursor.lockState = CursorLockMode.Locked; // if we're reloading, show error. ( also locks cursor for the editor )
            Cursor.visible = false;
            errorText.FourSecFade("Can't Open Menu While Reloading"); 
        }
    }

    [PunRPC]
    public void UpdatePlayerName(string name) {
        playerName.text = nickname;
    }
   
    public bool SetKeycodeValue(KeycodeFunction keycodeFunction, KeyCode keyCode)
    {
        //Debug.Log(keyCode + "：Setting Button logic for: " + inputSystemDic[keycodeFunction]);
        if (inputSystemDic.Values.Contains(keyCode))
        {
            //Debug.Log(keyCode + "：Button logic already exists");
            errorText.FourSecFade("That key is already in use.");
            //MenuManager.Instance.OpenMenu("error");
            //errorText.text = "That key is already in use";

            // The following code was written by Jacob Brown : 10/3/2022
            // this code swaps the keyCodes if a key has already been mapped to an action
            // You can remove the debugs whenever you like
      /*      KeyValuePair<KeycodeFunction, KeyCode>[] pairs;
            pairs = inputSystemDic.ToArray();
            KeycodeFunction tempFunction;
            KeyCode temp;*/
            
           /* for (int i = 0; i < pairs.Length; i++) {
                if (pairs[i].Value == keyCode) {
                    tempFunction = pairs[i].Key;
                    Debug.Log(tempFunction);
                    temp = inputSystemDic[keycodeFunction];
                    Debug.Log(temp);
                    Debug.Log("BEFORE: " + inputSystemDic[keycodeFunction]);
                    inputSystemDic[keycodeFunction] = keyCode;
                    Debug.Log("AFTER: " + inputSystemDic[keycodeFunction]);
                    Debug.Log("BEFORE: " + inputSystemDic[tempFunction]);
                    inputSystemDic[tempFunction] = temp;
                    Debug.Log("AFTER: " + inputSystemDic[tempFunction]);
                    break;
                }
            }*/
            // end Jacob Brown edits
            return true;
        }
        else
        {
            inputSystemDic[keycodeFunction] = keyCode;
            return false;
        }
    }
    public void SetTeams()
    {
       if (!PV.IsMine)
        {
            return;
        }
        //var player1 = PhotonNetwork.CurrentRoom.Players.ElementAt(0);
     
        GameObject[] ga = (GameObject.FindGameObjectsWithTag("Player"));
        //here's where I set the color of the players - zach
        List<PlayerDamageable> pd = new List<PlayerDamageable>();
        foreach (GameObject g in ga)
            if (g.GetComponent<PlayerDamageable>() != null)
            {
                //Debug.Log("adding" + g.GetComponent<PlayerDamageable>().gameObject.transform.parent.name);
                pd.Add(g.GetComponent<PlayerDamageable>());
                
            }
        GameObject[] newobj = new GameObject[pd.Count];
        PhotonView[] newPVArr = new PhotonView[newobj.Length];
        int k = 0;
        foreach (PlayerDamageable pdmg in pd)
        {
            newPVArr[k] = pdmg.gameObject.GetComponentInParent<PhotonView>();
            newobj[k] = pdmg.gameObject;
            k++;
           // Debug.Log("added player damagable for actor # " + newPVArr[k].OwnerActorNr);
        }
        foreach (Player pp in PhotonNetwork.PlayerList)
        {
          //  pp.NickName
            //Debug.Log(pp.NickName + " is in playerList");
            if (pp == null)
            {
                Debug.Log("null player?");
            }
            if ((int)pp.CustomProperties["team"] == 2)
            {
                //Debug.Log(pp.NickName + " was on team 2");
                //setMaterialTeam2(pp);
                setMaterial(pp, newobj, newPVArr);
            }
        }
    }
    public void setMaterial(Player p, GameObject[] newobj, PhotonView[] newPVArr)
    {
        int actorNumberToChange = p.ActorNumber;
        //Debug.Log("matching player found");
        for ( int i =0; i < newobj.Length; i++)
        {
            //Debug.Log(" p actor # " + p.ActorNumber + " vs " + newPVArr[i].OwnerActorNr);
            if (p.ActorNumber == newPVArr[i].OwnerActorNr)
            {
                MeshRenderer bodyMesh = newobj[i].gameObject.GetComponent<MeshRenderer>();
                //Debug.Log("body mesh: " + bodyMesh.name);
            
                MeshRenderer[] m = gameObject.GetComponentsInChildren<MeshRenderer>();

                //Debug.Log("got mesh = " + m[1].gameObject.name);
                Material[] materials = new Material[1];
                materials = bodyMesh.materials;
                materials[0] = (Material)Resources.Load("materials/Player_Mat1");
                //Debug.Log("size of materials arr = " + materials.Length);
                //Debug.Log(materials[0]);

                Material[] playerMaterials = materials;
                //Debug.Log("new player materials being set on mesh render = " + playerMaterials[0]);
                bodyMesh.gameObject.GetComponent<MeshRenderer>().materials = playerMaterials;
            }
        }
    }
}
public enum KeycodeFunction
{
    leftMove,
    rightMove,
    upMove,
    downMove,
    slowwalk,
    sprint,
    crouch,
    slide,
    jump,
    reload,
    scoreboard,
    menu,
    chatRoom
}