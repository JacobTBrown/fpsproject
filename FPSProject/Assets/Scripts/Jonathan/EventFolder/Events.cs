using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Scripts.Jonathan{
 /*
        For all Game Events Please add any New Events to this Script

    
        - Edited By Jonathan Alexander on 9/27/2022 11:33pm
 */
    public static class Events
    {
         public static PlayerDeathEvent PlayerDeathEvent = new PlayerDeathEvent();

    }

    public class PlayerDeathEvent : GameEvent { 
        public GameObject player;
    }

    
}
