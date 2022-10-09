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
        public static PickupEvent pickupEvent = new PickupEvent();
        public static ObjectiveUpdateEvent ObjectiveUpdateEvent = new ObjectiveUpdateEvent();
        public static AllObjectivesCompletedEvent allOjbectivesCompletedEvent = new AllObjectivesCompletedEvent();
        public static EnemyKillEvent enemyKillEvent = new EnemyKillEvent();
    }

    public class PickupEvent : GameEvent
    {
    }

    public class ObjectiveUpdateEvent : GameEvent
    {
    }

    public class AllObjectivesCompletedEvent : GameEvent
    {
    }

    public class PlayerDeathEvent : GameEvent { 
        public GameObject player;
    }
    public class EnemyKillEvent :GameEvent
    {

    }
    
}
