using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Unity.Scripts.Jonathan
{
	public class SpawnController : MonoBehaviour
	{
	    SpawnManager rManager;
		
		// we'll need a public variable to access spawn points
		// programatically, each player should recieve a spawn point, (not necessarily different)
		//   > we should be able to check for the most appropriate spawn based on some function such as
		//		"checkForPlayerInRange()" function the that keeps track of other player's position - in GameManager.cs possibly? idk
		//		not *quite* sure on the implementation quite yet, but we will need a static reference to this controller (probably in SpawnManager.cs or some Class that references the SpawnManager).
		//		note* functions that are not attatched to an instantiated object/prefab be executed by all players, so this will need to be adapted a little for multiplayer.
		//			** Instantiated objects are created at runtime, i.e., they will not exist in the heirarchy when the game is not executing.
		//			so, the spawn points themselvs will not be instantiated, but we will need access to them to instantiate the players.
		// -Zach

	    void Start(){
	
	        rManager = FindObjectOfType<SpawnManager>();
	        rManager.RegisterSpawnPoint(this);
	    }
	
	    public void SpawnPlayer(GameObject player){
			player.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
	        player.transform.position = transform.position + new Vector3(0,2,0);
	    }
	
	}
}