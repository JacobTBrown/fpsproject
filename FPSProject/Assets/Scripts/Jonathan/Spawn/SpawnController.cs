using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Unity.Scripts.Jonathan
{
	public class SpawnController : MonoBehaviour
	{
		SpawnManager rManager;

		SpawnController Instance;
		public void Awake()
        {
			this.gameObject.SetActive(true);
			if (Instance)
            {

				Debug.Log("already existed");
				return;
            }
			Instance = this;
			//Debug.Log("Spawn controller game obj : " + gameObject);
			//Debug.Log("Spawn controller game objname : " + gameObject.name);
        }
        void Start(){
			//if (Instance){}
			//Debug.Log("Running start in spawnctrl game obj: " + this.gameObject.name);
			//Debug.Log("Running start in spawnctrl game obj: " + this.gameObject.GetComponent<SpawnController>());
	        rManager = FindObjectOfType<SpawnManager>();
	        //rManager.RegisterSpawnPoint(this);
	        rManager.RegisterSpawnPoint(this.gameObject.GetComponent<SpawnController>());
	    }
	
	    public void SpawnPlayer(GameObject player){
			/*if (!this)
            {
				this.gameObject = 
				this.rManager = new SpawnManager();
            }*/
			//Debug.Log("Player was: " + player.GetComponent<PhotonView>().ViewID);
			player.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
	        player.transform.position = transform.position + new Vector3(0,2,0);
			//player.GetComponentInChildren<PlayerDamageable>().currentHealth = 100f; //moved to PlayerDamagable.cs
		}
	
	}
}