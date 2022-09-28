using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] float health = 100f;
    public void Damage(float damage)
    {
        health -= damage;

    }
}
