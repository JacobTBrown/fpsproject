using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Zach 10-10: MODEL / **VIEW** / CONTROLELR
//  This file renders the saved stats onto the screen.
//  Grabs info from playerStatsPage
//  PlayerStatsPage uses DataToStore to serialize the data,
//  and DataSaver stores the data to a local file
//  
public class PlayerStatsGui : MonoBehaviour
{
    public int toolbarInt = 1; //defaults to 1=hidden, 0=visible
    public string[] toolbarStrings; // = new string[] { "Show Panel", "Hide"}; setting these all in the inspector
    GameObject timeCounter;
    GameObject totalTimeCounter;
    GameObject levelCounter;
    GameObject fpsCounter;
    GameObject killsCounter;
    GameObject deathsCounter;
    private int buttonWidth = 100;
    private int buttonHeight = 50;
    public Text timeCounterText;
    public Text fpsCounterText;
    public Text levelCounterText;
    public Text totalTimeCounterText;
    public Text killsCounterText;
    public Text deathsCounterText;
    public TMP_Text pingCounterText;
    public PlayerStatsPage stats;

    /*[HideInInspector] //doesn't dynamically scale with resolution - maybe because of the canvas scaling on its own
    public int resolutionHeight;
    public int resolutionWidth;
    public int relativeResolutionHeight;
    public int relativeResolutionWidth;
    public int buttonPosition;*/

    private void Awake()
    {
        //timeCounter = gameObject.GetComponentInChildren<Text>();
        timeCounter = GameObject.Find("InGameTimerOverlay");
        totalTimeCounter = GameObject.Find("TimerOverlay");
        //totalTimeCounter = GameObject.Find("inGameTimer");
        //fpsCounter = GameObject.Find("FPS");
        fpsCounter = GameObject.Find("FPSOverlay");
        levelCounter = GameObject.Find("LevelOverlay");
        killsCounter = GameObject.Find("KillsOverlay");
        deathsCounter = GameObject.Find("DeathsOverlay");
        timeCounter.SetActive(false);
        totalTimeCounter.SetActive(false);
        fpsCounter.SetActive(false);
        killsCounter.SetActive(false);
        deathsCounter.SetActive(false);

        //Debug.Log("time counter: " + timeCounter.name);
        //Text timeCounterText = timeCounter.GetComponentInChildren<Text>();
        //Text totalTimeCounterText = totalTimeCounter.GetComponentInChildren<Text>();
        //Text fpsCounterText = fpsCounter.GetComponentInChildren<Text>();
        //Text killsCounterText = killsCounter.GetComponentInChildren<Text>();
        //Text deathsCounterText = deathsCounter.GetComponentInChildren<Text>();
        //Debug.Log("time counter text: " + timeCounterText.text);
    }
    void Start()
    {
        //stats.LoadPlayer();
        // resolutionHeight = Screen.currentResolution.height;
        //resolutionWidth = Screen.currentResolution.width ;
        //fpsCounter.GetComponent<>;

    }
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(timeCounterText.text);
        //Debug.Log(PlayerStatsPage.Instance.getTime().ToString());
        timeCounterText.text = "Time: " + stats.GetTime().ToString();
        totalTimeCounterText.text = "Total: " + stats.GetTotalTime().ToString();
        levelCounterText.text = "Level: " + stats.GetLevel().ToString();
        fpsCounterText.text = "FPS: " + stats.GetFPS().ToString();
        killsCounterText.text = "Kills: " + stats.GetKills().ToString();
        deathsCounterText.text = "Deaths: " +stats.GetDeaths().ToString();

      /*  resolutionHeight = Screen.currentResolution.height;
        resolutionWidth = Screen.currentResolution.height;
        relativeResolutionHeight = resolutionHeight;
        relativeResolutionWidth = resolutionWidth;
        buttonWidth = relativeResolutionWidth / 10;
        buttonHeight = relativeResolutionHeight / 10;
        Debug.Log("Resolution: " + relativeResolutionHeight + " X " + relativeResolutionWidth);
        Debug.Log("button size: " + buttonWidth + " X " + buttonHeight);*/
        //GUI.Toolbar(new Rect(25, 25, 250, 30), toolbarInt, toolbarStrings);
    }
    public int GetExp()
    {

        return 1;
    }
    private void OnGUI()
    {
        /*
        resolutionHeight = Screen.currentResolution.height;
          resolutionWidth = Screen.currentResolution.height;
          relativeResolutionHeight = resolutionHeight;
          relativeResolutionWidth = resolutionWidth;
          Debug.Log("Resolution: " + relativeResolutionHeight + " X " + relativeResolutionWidth);
          Debug.Log("button size: " + buttonWidth + " X " + buttonHeight);*/
        //Debug.Log("ONGUI CALL!!!");
        toolbarInt = GUI.Toolbar(new Rect(5, 5, 185, 50), toolbarInt, toolbarStrings);
        if (toolbarInt == 0)
        {
            GUI.Box(new Rect(5, 58, 110, 500), "Player Stats");
            //toolbarInt = GUI.Toolbar(new Rect(25, 25, 250, 30), toolbarInt, toolbarStrings);

            if (GUI.Button(new Rect(10, 100, buttonWidth, buttonHeight), "In-Game Time"))
            {
                if (timeCounter.activeInHierarchy)
                    timeCounter.SetActive(false);
                else timeCounter.SetActive(true);
            }
            if (GUI.Button(new Rect(10, 150, buttonWidth, buttonHeight), "Total Time"))
            {
                if (totalTimeCounter.activeInHierarchy)
                    totalTimeCounter.SetActive(false);
                else totalTimeCounter.SetActive(true);
            }
            else if (GUI.Button(new Rect(10, 150 + buttonHeight, buttonWidth, buttonHeight), "Level"))
            {
                if (levelCounter.activeInHierarchy)
                    levelCounter.SetActive(false);
                else levelCounter.SetActive(true);
            }
            else if (GUI.Button(new Rect(10, 150 + buttonHeight * 2, buttonWidth, buttonHeight), "FPS"))
            {
                if (fpsCounter.activeInHierarchy)
                    fpsCounter.SetActive(false);
                else fpsCounter.SetActive(true);
            }
            else if (GUI.Button(new Rect(10, 150 + buttonHeight * 3, buttonWidth, buttonHeight), "Ping"))
            {
                if (pingCounterText.IsActive()) //was set in the inspector
                {
                    pingCounterText.enabled = false;
                    pingCounterText.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                }
                else
                {
                    pingCounterText.enabled = true;
                    pingCounterText.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                }
            }
            else if (GUI.Button(new Rect(8, 150 + buttonHeight * 4, buttonWidth, buttonHeight), "kills"))
            {
                if (killsCounter.activeInHierarchy)
                    killsCounter.SetActive(false);
                else killsCounter.SetActive(true);
            }
            else if (GUI.Button(new Rect(8, 150 + buttonHeight * 5, buttonWidth, buttonHeight), "deaths"))
            {
                if (deathsCounter.activeInHierarchy)
                    deathsCounter.SetActive(false);
                else deathsCounter.SetActive(true);
            }
        }
    }
}
