using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class SaveUserName : MonoBehaviour
{
    string playerNamePrefKey = "PlayerName";
    //playerprefs - UnityAPI's PlayerPrefs holds key-value pairs, stored locally: https://gamedevbeginner.com/how-to-use-player-prefs-in-unity/ 
    // key: PlayerName, value: *input in WelcomeScreen textbox*
    // get the value of the key with value = PlayerPrefs.GetString("key"); or set the value of the key with PlayerPrefs.SetString("key", value);
    // Zach 10-12
    bool debug = true;
    
    private void Start()
    {
        string username = "YourNameHere";
        TMP_InputField inputField = gameObject.GetComponent<TMP_InputField>();
        if (inputField != null)
        if (PlayerPrefs.HasKey(playerNamePrefKey))
        {
                if (debug) Debug.Log("name found");
                username = PlayerPrefs.GetString(playerNamePrefKey);
                inputField.text = username;
        }
        PhotonNetwork.NickName = username;
    }
    public void OnInputChanged(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            if (debug) Debug.Log("name cannot be null");
            return;
        }
            if (debug) Debug.Log("input changed");
            PlayerPrefs.SetString(playerNamePrefKey, name);
            PhotonNetwork.NickName = name;
    
    }
}
