using Photon.Pun;
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
			Debug.Log("Player was: " + player.GetComponent<PhotonView>().ViewID);
			player.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
	        player.transform.position = transform.position + new Vector3(0,2,0);
			//player.GetComponentInChildren<PlayerDamageable>().currentHealth = 100f; //moved to PlayerDamagable.cs
		}
	
	}
}