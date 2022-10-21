﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Scripts.Jonathan;
using UnityEngine;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] public float currentHealth;
    public float maxHealth = 100f;
    public bool isInvincible = false;
    public bool isSpeed = false;
    public HealthBar healthBar;
    public Animator DamageFlash;
    public GameObject player;
    public PhotonView PV;
    AudioSource impact;

    void Awake()
    {
        player = transform.parent.gameObject;
        PV = player.GetComponent<PhotonView>();
    }
    void Start()
    {

        //Debug.Log(PV.name.ToString());
        if (PV.IsMine)
        {
            //Debug.Log("Starting player damage");
            impact = gameObject.GetComponent<AudioSource>();
            DamageFlash = GameObject.Find("Canvas10-14").GetComponentInChildren<Animator>();

            healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
            Debug.Log("Healthbar is: " + healthBar.name);
            healthBar.SetMaxHealth(maxHealth);
            currentHealth = maxHealth;
        }
        /*    impact = GetComponent<AudioSource>();
            DamageFlash = GameObject.Find("DamageFlash").GetComponent<Animator>();
            healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
            healthBar.SetMaxHealth(maxHealth);
            currentHealth = maxHealth;
    */
        //healthBar.SetMaxHealth(100);
        //Debug.Log(currentHealth);
        //}
        //Debug.Log("Player damageable started with objects: dmgFlash: " + DamageFlash.gameObject + " HealthBar: " + healthBar.gameObject);
    }

    void Update()
    {
        if (!PV.IsMine)
        {
           // Debug.Log("not your pv");
            return;
        }
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Debug.Log("TEST DAMAGE KEY PRESSED, PLAYER TAKES 20 DAMAGE!");
            Damage(20f);
        }
        healthBar.SetHealth(currentHealth, PV);
        if (isInvincible) healthBar.changeColor(PV, Color.blue);
        else if (isSpeed) healthBar.changeColor(PV, Color.green);
        else healthBar.changeColor(PV, Color.red);

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("TEST: NEW PLAYER KILL EVENT FOR " + player);
            PlayerKillEvent playerKillEvent = Events.PlayerKillEvent;
            playerKillEvent.player = player;
            EventManager.Broadcast(playerKillEvent);
        }
    }

    public void Damage(float damage)
    {
        if(PV.IsMine)
        {
            if (!isInvincible)
            {
                currentHealth -= damage;
                impact.Play();
                DamageFlash.SetTrigger("Damage");
                healthBar.SetHealth(currentHealth, PV);
                Debug.Log("Hit Player for " + damage + " damage. Player is now at " + currentHealth + " HP.");
            }
        }
        if (currentHealth <= 0)
        {

            PlayerDeathEvent evt = Events.PlayerDeathEvent;
            if (PV.IsMine)
            {
                evt.player = player;
                currentHealth = 100;
                healthBar.SetHealth(currentHealth, PV);
                EventManager.Broadcast(evt);
                
            }
            
            Debug.Log("A player has died!");
            
            //Destroy(transform.gameObject);
            //chaging to jonathan's script
        }
    }
}
