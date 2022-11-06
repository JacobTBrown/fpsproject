using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Unity.Scripts.Jonathan
{
	public class SpawnController : MonoBehaviour
	{
		SpawnManager rManager;

		public bool PlayerNearby = false;
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

        }
        void Start(){

	        rManager = FindObjectOfType<SpawnManager>();
	        rManager.RegisterSpawnPoint(this.gameObject.GetComponent<SpawnController>());
	    }
	
	    public void SpawnPlayer(GameObject player){

			player.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
	        player.transform.position = transform.position + new Vector3(0,2,0);
		}

		void OnCollisionEnter(Collision collision)
		{
		//	if(collision.gameObject.name)
				PlayerNearby = true;
		}

		void OnCollisionStay(Collision collision)
		{
		//	if(collision.gameObject.name)
				PlayerNearby = true;
		}

		void OnCollisionExit(Collision collision)
		{
		//	if(collision.gameObject.name)
				PlayerNearby = false;
		}
	}
}