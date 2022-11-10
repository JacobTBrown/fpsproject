using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Scripts.Jonathan
{
	
	public class SpawnManager : MonoBehaviour
	{
		//we'll need to access this elsewhere for the multiplayer
	    public List<SpawnController> SpawnPoints;
	    
	    public int NumberofSpawnPoints = 0;
		public SpawnStrategy strategy{get;set;}
		public static SpawnManager Instance;

	    void Awake()
	    {
			
			if (Instance)
				Debug.Log("Instance already existed");
			Instance = this;
			
	        SpawnPoints = new List<SpawnController>();

	        EventManager.AddListener<PlayerSpawnEvent>(OnPlayerSpawn);

	        strategy = new RandomSpawnStrategy();

	    }
	
	    public void RegisterSpawnPoint(SpawnController SpawnPoint)
	    {
			//Debug.Log("Register Spawn Point: " );
	        SpawnPoints.Add(SpawnPoint);
	        NumberofSpawnPoints++;
	    }
	
	    void OnPlayerSpawn(PlayerSpawnEvent evt){
	        HandlePlayerSpawn(evt.player);
	    }

	
	    public void HandlePlayerSpawn(GameObject player){
	        strategy.HandlePlayerSpawn(player, SpawnPoints);
	    } 
	}
}