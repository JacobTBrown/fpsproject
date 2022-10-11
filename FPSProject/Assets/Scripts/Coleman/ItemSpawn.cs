using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public static GameObject[] spawnList;
    private GameObject itemSpawn;
    private float spawnDelay = 10;
    private float nextSpawnTime;
    private bool hasSpawned;
    // Start is called before the first frame update
    void Start()
    {
        spawnList = Resources.LoadAll<GameObject>("ItemPrefabs");
        hasSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldSpawn())
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        nextSpawnTime = Time.time + spawnDelay; 
        itemSpawn = spawnList[Random.Range(0,spawnList.Length)];
        Instantiate(itemSpawn, transform.position + new Vector3(0,1,0), transform.rotation);
    }

    private bool shouldSpawn()
    {
        if (hasSpawned) return false;
        else return Time.time >= nextSpawnTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            hasSpawned = true;
            Debug.Log(hasSpawned);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            hasSpawned = false;
            Debug.Log(hasSpawned);
        }
    }
}
