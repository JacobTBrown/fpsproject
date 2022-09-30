using System;
using UnityEngine;
using UnityEngine.UI;

public class DownAnyKey : MonoBehaviour
{
    Button button;
    Text inputtext;
    public KeyCode curKetCode;
    [HideInInspector]
    public bool IsChangeKey = false; //是否改键位
    public KeycodeFunction keycodeFunction;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);

        inputtext = GetComponentInChildren<Text>();
        inputtext.text = curKetCode.ToString();
        buttonColor = button.image.color;
    }
    // Update is called once per frame
    void Update()
    {
        if (IsChangeKey)
            getKeyDownCode();
    }
    void ButtonClicked()
    {
        IsChangeKey = true;
        button.image.color = Color.black;
    }
    Color buttonColor;
    void ButtonUnSelect()
    {
        button.image.color = buttonColor;
    }
    public void getKeyDownCode()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode) && keyCode != KeyCode.Mouse0)
                {
                    if (keyCode == KeyCode.Escape) break;
                    curKetCode = keyCode;
                    bool isExist = GameObject.Find("Player(Clone)").GetComponent<PlayerSettings>().SetKeycodeValue(keycodeFunction, curKetCode); // to convert this to multiplayer, we should instantiate the player prefab with it's PhotonId, and use Find("Player" + Photon.Network.playerID).
                    if (isExist) return;
                    ButtonUnSelect();
                    inputtext.text = keyCode.ToString();
                }  
            }
            IsChangeKey = false;
            ButtonUnSelect();
        }
    }
    public void SetKeyValue(int mKey)
    {
        RecoverState();
        this.curKetCode = (KeyCode)mKey;
        inputtext.text = curKetCode.ToString();
    }
    public void RecoverState()
    {
        IsChangeKey = false;
        ButtonUnSelect();
    }
}