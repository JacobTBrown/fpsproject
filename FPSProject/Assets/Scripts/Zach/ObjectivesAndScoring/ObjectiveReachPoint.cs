using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveReachPoint : MonoBehaviour //Objective
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
        if (captured)
        {
            return;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

