using System;
using UnityEngine;
using UnityEngine.UI;

/*
    Author: Peifan Tian
    Creation: 9/19/22
    Last Edit: 9/23/22 -Peifan

*/

public class DownAnyKey : MonoBehaviour
{
    Button button;
    Text inputtext;
    public KeyCode curKetCode;
    [HideInInspector]
    public bool IsChangeKey = false; 
    public KeycodeFunction keycodeFunction;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);

        inputtext = GetComponentInChildren<Text>();
        inputtext.text = curKetCode.ToString();
        buttonColor = button.image.color;
    }
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
                    bool isExist = GameObject.Find("Player(Clone)").GetComponent<PlayerSettings>().SetKeycodeValue(keycodeFunction, curKetCode); 
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