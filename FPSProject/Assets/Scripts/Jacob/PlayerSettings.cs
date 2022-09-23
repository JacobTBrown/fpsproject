using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Author: Jacob Brown
    Creation: 9/19/22
    Last Edit: 9/21/22

    A class representing all of the actions that a player can take along 
    with the default key-mappings associated with said actions.
*/
public class PlayerSettings : MonoBehaviour
{
    [Header("User Mouse Settings")]
    // Horizontal mouse sensitivity
    public float mouseXSensitivity = 250f;
    // Vertical mouse sensitivity
    public float mouseYSensitivity = 250f;
    // For the user to invert the Y mouse look
    public bool invertMouse = false;
    [Header("User Keybinds")]
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode sprint = KeyCode.LeftShift;
    public KeyCode jump = KeyCode.Space;
    public KeyCode reload = KeyCode.R;
    public KeyCode scoreboard = KeyCode.Tab;
    public KeyCode menu = KeyCode.Escape;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetKeybinds() {

    }
}
