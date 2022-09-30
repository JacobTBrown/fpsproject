using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Scripts.Jonathan
{
    public interface SpawnStrategy
    {
        void HandlePlayerSpawn(GameObject player, List<SpawnController> SpawnPoints);
    }
}
