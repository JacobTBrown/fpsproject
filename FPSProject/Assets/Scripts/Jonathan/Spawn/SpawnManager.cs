﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Scripts.Jonathan
{
	public class SpawnManager : MonoBehaviour
	{
	    public List<SpawnController> SpawnPoints;
	    
	    public int NumberofSpawnPoints = 0;
	    public SpawnStrategy strategy;
		public static SpawnManager Instance;

	    void Awake()
	    {
			Instance = this;
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