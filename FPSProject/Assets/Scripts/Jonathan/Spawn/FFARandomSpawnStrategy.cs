using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Unity.Scripts.Jonathan
{
	public class FFARandomSpawnStrategy : SpawnStrategy
	{
		
		public Vector3 HandlePlayerSpawn(GameObject player, List<SpawnController> SpawnPoints)
		{
            SpawnController selectedSpawnPoint;
            List<SpawnController> LegalSpawnControllers = new List<SpawnController>();
            foreach(SpawnController spawnPoint in SpawnPoints)
            {
                if(spawnPoint.PlayerNearby == false)
                    LegalSpawnControllers.Add(spawnPoint);
            }
            if(LegalSpawnControllers.Count > 0)
			     selectedSpawnPoint = LegalSpawnControllers[Random.Range(0, LegalSpawnControllers.Count)];
            else 
                 selectedSpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Count)];

			selectedSpawnPoint.SpawnPlayer(player);
			
			return selectedSpawnPoint.transform.position;
		}
	}
}
