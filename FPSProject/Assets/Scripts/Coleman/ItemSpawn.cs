using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public static GameObject[] spawnList;
    private GameObject itemSpawn;
    private float spawnDelay = 60;
    private float nextSpawnTime;
    // Start is called before the first frame update
    void Start()
    {
        spawnList = Resources.LoadAll<GameObject>("ItemPrefabs");
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
        itemSpawn = spawnList[Random.Range(0,spawnList.Length-1)];
        Instantiate(itemSpawn, transform.position + new Vector3(0,1,0), transform.rotation);
    }

    private bool shouldSpawn()
    {
        return Time.time >= nextSpawnTime;
    }
}
