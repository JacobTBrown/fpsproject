using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] public float currentHealth;
    public float maxHealth = 100f;
    public HealthBar healthBar;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
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
            Debug.Log("Player is dead!");
            Destroy(transform.gameObject);
        }
    }
}
