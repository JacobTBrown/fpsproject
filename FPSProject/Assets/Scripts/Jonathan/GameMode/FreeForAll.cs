using System.Collections;
using System.Collections.Generic;
using Unity.Scripts.Jonathan;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
/*
    - Creates Objectives and Rules assosicated with a Free for All game
    - Sends rules to Objective and Rules to the RuleManager and ObjectiveManager 
*/
public class FreeForAll : MonoBehaviour, GameMode
{
    int killCutOff = 10;

   // ObjectiveManager o_Manager = ObjectiveManager.Instance;
  //  RuleManager r_Manager = RuleManager.Instance;
    ObjectiveManager o_Manager;
    RuleManager r_Manager;
    GameManager g_Manager;
    void Awake(){
        o_Manager = GameObject.Find("GameManager").GetComponent<ObjectiveManager>();
        r_Manager = GameObject.Find("GameManager").GetComponent<RuleManager>();  
        g_Manager = GameObject.Find("GameManager").GetComponent<GameManager>();  
    }
    public FreeForAll()
    {      
    }

    public FreeForAll(int killCutOff)
    {
        this.killCutOff = killCutOff;        
    }

    public void CreateGameRules()
    {
          g_Manager.gameObject.AddComponent<FreeForAllRule>();
    }
    
    public void CreateGameObjectives()
    {
        g_Manager.gameObject.AddComponent<FreeForAllKillObjective>();
    }

    public void LoadGameModeUI()
    {
        //#Find Canvas
        //#Find FFA UI in Resources
        //#instantiate FFA into Cavas
    }




}
