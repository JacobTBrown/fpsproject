using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static Dictionary<int, ItemSpawner> spawners = new Dictionary<int, ItemSpawner>();
    private static int spawnerId = 1;
    public bool hasItem = false;
    // Start is called before the first frame update
    void Start()
    {
        hasItem = false;
        spawners.Add(spawnerId++, this);
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           // Player _player = other.GetComponent<PhotonView>();
        }
    }
    private IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(10f);
        hasItem = true;
    }
    private void ItemPickedUp(int _byPlayer)
    {
        hasItem = false;

        StartCoroutine(SpawnItem());
    }
}
