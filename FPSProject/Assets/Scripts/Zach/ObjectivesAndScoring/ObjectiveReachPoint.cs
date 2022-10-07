using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveReachPoint : Objective
{
    [Tooltip("Transform for the point")]
    public Transform ReturnPoint;
    //initializ
    public bool captured;

    private void Awake()
    {
        if (ReturnPoint == null)
        {
            ReturnPoint = transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        captured = true;
        if (captured)
        {
            Debug.Log("Captured");
            //instantiate a new prefab here? or in update?
            return;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //check to see if the game object exists: check if the player has a copy of it
        //Instantiate a new flag prefab when the player dies or scores
    }
}

