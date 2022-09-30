using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Unity.Scripts.Jonathan
{
	public class SpawnController : MonoBehaviour
	{
	    SpawnManager rManager;
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