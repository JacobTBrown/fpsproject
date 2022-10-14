using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityPowerup : MonoBehaviour
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
        if (other.CompareTag("Player"))
        {
            StartCoroutine(HealthPickup(other));
        }
    }

    IEnumerator HealthPickup(Collider player)
    {
        PlayerDamageable stats = player.GetComponent<PlayerDamageable>();
        if (!stats.isInvincible)
        {
            pickupEffect.Play();
            powerupSound.Play();
            foreach (Transform child in parent.gameObject.transform)
            {
                child.GetComponent<MeshRenderer>().enabled = false;
            }
            stats.isInvincible = true;
            transform.parent.position = new Vector3(0, -100, 0);
            yield return new WaitForSeconds(1);
            GetComponent<Collider>().enabled = false;
            yield return new WaitForSeconds(14);
            stats.isInvincible = false;
            Destroy(transform.parent.gameObject);
        }
    }
}
