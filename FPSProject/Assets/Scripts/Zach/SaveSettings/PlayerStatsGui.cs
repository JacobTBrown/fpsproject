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
    public string toolbarString; // = new string[] { "Show Panel", "Hide"}; setting these all in the inspector
    GameObject timeCounter;
    GameObject totalTimeCounter;
    GameObject levelCounter;
    GameObject fpsCounter;
    GameObject killsCounter;
    GameObject deathsCounter;
    GameObject KDRCounter;
    private int buttonWidth = 100;
    private int buttonHeight = 50;
    public Text timeCounterText; //set these in the inspector
    public Text fpsCounterText;
    public Text levelCounterText;
    public Text totalTimeCounterText;
    public Text killsCounterText;
    public Text deathsCounterText;
    public Text KDRCounterText;
    public TMP_Text pingCounterText;
    public PlayerStatsPage stats;
    public double kdr;
    private bool toolbarBool;
    public int inc = 4; //gets set in the inspector tho
    public int init; 
    /*[HideInInspector] //doesn't dynamically scale with resolution - maybe because of the canvas scaling on its own
    public int resolutionHeight;
    public int resolutionWidth;
    public int relativeResolutionHeight;
    public int relativeResolutionWidth;
    public int buttonPosition;*/

    private void Awake()
    {

        toolbarString = "toggle stats";
        //timeCounter = gameObject.GetComponentInChildren<Text>();
        timeCounter = GameObject.Find("InGameTimerOverlay");
        totalTimeCounter = GameObject.Find("TimerOverlay");
        //totalTimeCounter = GameObject.Find("inGameTimer");
        //fpsCounter = GameObject.Find("FPS");
        fpsCounter = GameObject.Find("FPSOverlay");
        levelCounter = GameObject.Find("LevelOverlay");
        killsCounter = GameObject.Find("KillsOverlay");
        deathsCounter = GameObject.Find("DeathsOverlay");
        KDRCounter = GameObject.Find("KDROverlay");

        timeCounter.SetActive(false);
        totalTimeCounter.SetActive(false);
        levelCounter.SetActive(false);
        fpsCounter.SetActive(false);
        killsCounter.SetActive(false);
        deathsCounter.SetActive(false);
        KDRCounter.SetActive(false);
        
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
    
        if (stats.GetDeaths() == 0)
        {
            KDRCounterText.text = "KDR: " + stats.GetKills().ToString();
        }
        else {
            double kdr = ((double)stats.GetKills() / (double)stats.GetDeaths());
            KDRCounterText.text = kdr.ToString("F") + " kdr";
            //Debug.Log(kdr.ToString("F"));
        }
        init = 95;
    }
    void Update()
    {
        //Debug.Log(timeCounterText.text);
        //Debug.Log(PlayerStatsPage.Instance.getTime().ToString());
       
        timeCounterText.text = "In-Game: " + SecondsToMinutes(stats.GetTime());
        totalTimeCounterText.text = "Total: " + SecondsToMinutes(stats.GetTotalTime());
        levelCounterText.text = "Level: " + stats.GetLevel().ToString();
        fpsCounterText.text = "FPS: " + stats.GetFPS().ToString();
        killsCounterText.text = "Kills: " + stats.GetKills().ToString();
        deathsCounterText.text = "Deaths: " +stats.GetDeaths().ToString();
        
    }
    public void OnClickToggleStats()
    {
        if (toolbarInt == 0) toolbarInt = 1; else if (toolbarInt == 1) toolbarInt = 0;
    }
    public string SecondsToMinutes(int sec)
    {
        
        int minutes = sec / 60; 
        string minutesFormat = minutes.ToString();
        minutesFormat += ":" + (sec % 60).ToString(); 
        //Debug.Log("Converting seconds to minutes" + minutesFormat);
        return minutesFormat;
    }
    private void OnGUI()
    {
        //toolbarBool = GUI.Toggle(new Rect(555, 555, 800, 500), toolbarBool, toolbarString);
        //toolbarInt = GUI.Toolbar(new Rect(5, 5, 185, 50), toolbarInt, toolbarStrings);
        if (toolbarInt == 0)
        {
            //GUI.Box(new Rect(5+ inc, 58, 109, 450), " Player Stats");

            if (GUI.Button(new Rect(10 + inc, init, buttonWidth, buttonHeight), "In-Game Time"))
            {
                if (timeCounter.activeInHierarchy)
                    timeCounter.SetActive(false);
                else timeCounter.SetActive(true);
            }
            if (GUI.Button(new Rect(10 + inc, init + buttonHeight, buttonWidth, buttonHeight), "Total Time"))
            {
                if (totalTimeCounter.activeInHierarchy)
                    totalTimeCounter.SetActive(false);
                else totalTimeCounter.SetActive(true);
            }
            else if (GUI.Button(new Rect(10 + inc, init + buttonHeight*2, buttonWidth, buttonHeight), "Level"))
            {
                if (levelCounter.activeInHierarchy)
                    levelCounter.SetActive(false);
                else levelCounter.SetActive(true);
            }
            else if (GUI.Button(new Rect(10 + inc, init + buttonHeight * 3, buttonWidth, buttonHeight), "FPS"))
            {
                if (fpsCounter.activeInHierarchy)
                    fpsCounter.SetActive(false);
                else fpsCounter.SetActive(true);
            }
            else if (GUI.Button(new Rect(10 + inc, init + buttonHeight * 4, buttonWidth, buttonHeight), "Ping"))
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
            
            else if (GUI.Button(new Rect(10 + inc, init + buttonHeight * 5, buttonWidth, buttonHeight), "Kills"))
            {
                if (killsCounter.activeInHierarchy)
                    killsCounter.SetActive(false);
                else killsCounter.SetActive(true);
            }
            else if (GUI.Button(new Rect(10 + inc, init + buttonHeight * 6, buttonWidth, buttonHeight), "Deaths"))
            {
                if (deathsCounter.activeInHierarchy)
                    deathsCounter.SetActive(false);
                else deathsCounter.SetActive(true);
            }
            else if (GUI.Button(new Rect(10 + inc, init + buttonHeight * 7, buttonWidth, buttonHeight), "KDR"))
            {
                if (KDRCounter.activeInHierarchy)
                    KDRCounter.SetActive(false);
                else KDRCounter.SetActive(true);
            }
        }
    }
}
