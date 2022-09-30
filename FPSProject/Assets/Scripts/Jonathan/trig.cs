using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Scripts.Jonathan;

public class trig : MonoBehaviour
{
    private GameObject t;
    void Start()
    {
        this.t = gameObject;
    }

    private void OnTriggerEnter(Collider other){

        PlayerDeathEvent evt = Events.PlayerDeathEvent;
        evt.player = t;
        EventManager.Broadcast(evt);
    }
}
