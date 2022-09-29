using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Scripts.Jonathan
{
	public class RandomSpawnStrategy : SpawnStrategy
	{
	/*
	    Basic Respawn Strategy
	    IN: Player GameObject, List of SpawnPoint GameObjects
	    Function: Selects random SpawnPoint From List and transforms player above the position
	
	*/
	    public void HandlePlayerSpawn(GameObject player, List<SpawnController> SpawnPoints){
	        SpawnController selectedSpawnPoint = SpawnPoints[Random.Range(0,SpawnPoints.Count)];
	
	        selectedSpawnPoint.SpawnPlayer(player);
	    }
	}
}
