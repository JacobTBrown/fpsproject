﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    private float health = 100f;
    public void Damage(float damage)
    {
        health -= damage;
        Debug.Log("Hit Target for " + damage + " damage. Target is now at " + health + " HP.");
        if (health <= 0)
        {
            Debug.Log("Target Destroyed!");
            Destroy(transform.parent.gameObject);
        }
    }
}