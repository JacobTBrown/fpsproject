using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Scripts.Jonathan
{
	public class FFASpawnStrategy : SpawnStrategy
	{
	/*
	    Basic Respawn Strategy
	    IN: Player GameObject, List of SpawnPoint GameObjects
	    Function: Selects random SpawnPoint if their are no Players Near by
	
	*/
		public Vector3 HandlePlayerSpawn(GameObject player, List<SpawnController> SpawnPoints)
		{
            // Idea: Go through All SpawnPoints and put All Spawn Points that are not
            //       Near by to a another list then select an random one
            List<SpawnController> ValidSpawnPoints = new List<SpawnController>();
            SpawnController selectedSpawnPoint;
            foreach(SpawnController spawnPoint in SpawnPoints)
            {
                if(!spawnPoint.PlayerNearBy) ValidSpawnPoints.Add(spawnPoint);
            }

            if(ValidSpawnPoints.Count > 0) selectedSpawnPoint = ValidSpawnPoints[Random.Range(0, ValidSpawnPoints.Count)];
            else  selectedSpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Count)];

			selectedSpawnPoint.SpawnPlayer(player);
			
			return selectedSpawnPoint.transform.position;
		}
	}
}