using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEvent : MonoBehaviour
{
    public PhotonView PV;
    public PlayerDamageable damageable;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    
    void Start()
    {
        damageable = GetComponentInChildren<PlayerDamageable>();
    }
    

    [PunRPC]
    public void DamagePlayer(float damage, int EnemyPlayer)
    {
        damageable.Damage(damage, EnemyPlayer);
    }
}
