using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Scripts.Jonathan;
using UnityEngine;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] public float currentHealth;
    public float maxHealth = 100f;
    public HealthBar healthBar;
    public GameObject player;
    public PhotonView PV;

    void Start()
    {
        
        player = transform.parent.gameObject;
        currentHealth = maxHealth;
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();

        healthBar.SetMaxHealth(maxHealth);
        PhotonView PV = player.GetComponent<PhotonView>();
        Debug.Log(PV.name.ToString());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Debug.Log("TEST DAMAGE KEY PRESSED, PLAYER TAKES 20 DAMAGE!");
            Damage(20f);
        }
        healthBar.SetHealth(currentHealth);
    }
    public void Damage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        Debug.Log("Hit Player for " + damage + " damage. Player is now at " + currentHealth + " HP.");
        if (currentHealth <= 0)
        {

            PlayerDeathEvent evt = Events.PlayerDeathEvent;
            if (PV.IsMine)
            {
                evt.player = player;
            
                EventManager.Broadcast(evt);
            }
            Debug.Log("A player has died!");
            
            //Destroy(transform.gameObject);
            //chaging to jonathan's script
        }
    }
}
