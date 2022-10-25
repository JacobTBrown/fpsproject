using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEvent : MonoBehaviour
{
    public PhotonView PV;
    public PlayerDamageable damageable;
    void Start()
    {
        damageable = GetComponentInChildren<PlayerDamageable>();
    }
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void DamagePlayer(float damage)
    {
        damageable.Damage(damage);
    }
}
