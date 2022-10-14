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
        public static PlayerKillEvent PlayerKillEvent = new PlayerKillEvent();
        public static NewPlayerEvent NewPlayerEvent = new NewPlayerEvent();
        public static PlayerSpawnEvent PlayerSpawnEvent = new PlayerSpawnEvent();
        public static PickupEvent pickupEvent = new PickupEvent();

        public static ObjectiveUpdateEvent ObjectiveUpdateEvent = new ObjectiveUpdateEvent();
        public static ObjectiveCompletedEvent objectiveCompletedEvent = new ObjectiveCompletedEvent();
        public static AllObjectivesCompletedEvent allOjbectivesCompletedEvent = new AllObjectivesCompletedEvent();
        public static EndGameEvent EndGameEvent = new EndGameEvent();
      
        public static EnemyKillEvent enemyKillEvent = new EnemyKillEvent();
        public static DamageEvent damageEvent = new DamageEvent();
    }

    public class PickupEvent : GameEvent
    {
    }
    
    public class PlayerKillEvent: GameEvent
    {
        public GameObject player;
    }

    public class PlayerSpawnEvent : GameEvent
    {
        public GameObject player;
    }

    public class NewPlayerEvent: GameEvent
    {
        public GameObject player;
    }
    public class ObjectiveUpdateEvent : GameEvent
    {
    }

    public class ObjectiveCompletedEvent: GameEvent
    {
       public Objective objective;
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

    public class EndGameEvent: GameEvent
    {

    }

    public class DamageEvent : GameEvent
    {
        float damage;
        GameObject EnemyPlayer;

    }
    
}
