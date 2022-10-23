using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Scripts.Jonathan;
/*
    Author: Jonathan Alexander
    Created: 10/12/22
    Updated: 10/13/22

    About: Objective Manager tracks all Game objectives and handles event calls for Objectives
           Objective Manager's Objectives are created by Gamemode

*/
public class ObjectiveManager : MonoBehaviour
{
    [SerializeField]List<Objective> objectiveList;

    public static ObjectiveManager Instance;

    void Awake(){

        Instance = this;
        objectiveList = new List<Objective>();

        EventManager.AddListener<PlayerKillEvent>(onGameEvent);
        EventManager.AddListener<ObjectiveCompletedEvent>(onCompletedObjective);
    }

    public void onGameEvent(PlayerKillEvent evt){
        for(int i = 0; i < objectiveList.Count;i++)
        {
            objectiveList[i].handleEvent(evt);
        }
    }

    public void onCompletedObjective(ObjectiveCompletedEvent evt){
        removeObjective(evt.objective);
    }

    public void addObjective(Objective obj){
        objectiveList.Add(obj);
        //Debug.Log("New Game Objective Added" + obj);
    }

    public void removeObjective(Objective obj){
        objectiveList.Remove(obj);
    }
}
