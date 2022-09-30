using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Scripts.Jonathan
{
	public class SpawnManager : MonoBehaviour
	{
	    private List<SpawnController> SpawnPoints;
	    
	    public int NumberofSpawnPoints = 0;
	    public SpawnStrategy strategy;
	   
	    void Awake()
	    {
	        SpawnPoints = new List<SpawnController>();
	        EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
	        strategy = new RandomSpawnStrategy();
	    }
	
	    public void RegisterSpawnPoint(SpawnController SpawnPoint)
	    {
	        SpawnPoints.Add(SpawnPoint);
	        NumberofSpawnPoints++;
	    }
	
	    void OnPlayerDeath(PlayerDeathEvent evt){
	        HandlePlayerSpawn(evt.player);
	    }
	
	    public void HandlePlayerSpawn(GameObject player){
	        strategy.HandlePlayerSpawn(player, SpawnPoints);
	    } 
	}
}