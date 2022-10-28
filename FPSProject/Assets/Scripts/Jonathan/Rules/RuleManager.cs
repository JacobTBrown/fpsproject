using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Scripts.Jonathan
{
    /*
       - Attached to GameManager Prefab
       - Adds Rules to a Rules list
            Rules typically come from GameMode
       - Sends ObjectiveCompletedEvent to all Rules
    */
    public class RuleManager : MonoBehaviour
    {
        List<Rule> ruleList;
        public static RuleManager Instance;


        void Awake()
        {
            EventManager.AddListener<ObjectiveCompletedEvent>(onCompletedObjective);
            Instance = this;
        }

        public void Init() {
            ruleList = new List<Rule>();
        }

        public void onCompletedObjective(ObjectiveCompletedEvent evt)
        {

        }
        public void addRule(Rule r)
        {
            //Debug.Log("NEW RULE: " + r);
            ruleList.Add(r);
        }

        public void removeRule(Rule rule)
        {
            ruleList.Remove(rule);
        }
    }
}