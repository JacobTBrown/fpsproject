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

		[SerializeField] public bool PlayerNearBy = false;
		public void Awake()
        {
			this.gameObject.SetActive(true);
			if (Instance)
            {

				//Debug.Log("already existed");
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

		private void OnTriggerExit(Collider other)
		{
			if(PlayerNearBy) PlayerNearBy = false;
		}

		private void OnTriggerStay(Collider other)
		{
			if(!PlayerNearBy) PlayerNearBy = true;
		}
	}
}