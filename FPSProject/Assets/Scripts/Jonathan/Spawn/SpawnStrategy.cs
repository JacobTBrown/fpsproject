using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Scripts.Jonathan
{
    public interface SpawnStrategy
    {
        Vector3 HandlePlayerSpawn(GameObject player, List<SpawnController> SpawnPoints);
    }
}
