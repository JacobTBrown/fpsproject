using System.Collections;
using System.Collections.Generic;
using Unity.Scripts.Jonathan;
using UnityEngine;


public interface Objective 
{
    void handleEvent(PlayerKillEvent evt);
}
