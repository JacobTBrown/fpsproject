using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private ParticleSystem pickupEffect;
    public AudioSource powerupSound;
    public GameObject parent;

    public void Start()
    {
        powerupSound = GetComponent<AudioSource>();
        parent = transform.parent.gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(Pickup(other));
        }
    }

    IEnumerator Pickup(Collider player)
    {
        PlayerDamageable stats = player.GetComponent<PlayerDamageable>();
        if (stats.currentHealth < stats.maxHealth)
        {
            pickupEffect.Play();
            powerupSound.Play();
            foreach (Transform child in parent.gameObject.transform)
            {
                child.GetComponent<MeshRenderer>().enabled = false;
            }
            if (stats.currentHealth < (stats.maxHealth / 2)) stats.currentHealth += 50f;
            else stats.currentHealth += stats.maxHealth - stats.currentHealth;
            transform.parent.position = new Vector3(0, 1000, 0);
            yield return new WaitForSeconds(1);
            GetComponent<Collider>().enabled = false;
            yield return new WaitForSeconds(3);
            Destroy(transform.parent.gameObject);
        }
    }
}
