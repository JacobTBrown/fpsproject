using System.Collections;
using System.Collections.Generic;
using Unity.Scripts.Jonathan;
using UnityEngine;
/*
    - Creates Objectives and Rules assosicated with a Free for All game
    - Sends rules to Objective and Rules to the RuleManager and ObjectiveManager 
*/
public class FreeForAll : MonoBehaviour, GameMode
{
    int killCutOff = 10;

    ObjectiveManager o_Manager = ObjectiveManager.Instance;
    RuleManager r_Manager = RuleManager.Instance;

    void Awake()
    {
        o_Manager = GameObject.Find("GameManager").GetComponent<ObjectiveManager>();
        r_Manager = GameObject.Find("GameManager").GetComponent<RuleManager>();
        Debug.Log(r_Manager);
        EventManager.AddListener<NewPlayerEvent>(onNewPlayer);
        createGameRules();
        
    }

    void createGameRules()
    {
        r_Manager.addRule(new FreeForAllRule());
    }

    public void onNewPlayer(NewPlayerEvent evt)
    {
        o_Manager.addObjective(new PlayerKillObjective(evt.player,killCutOff));
    }

}
