using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerup : MonoBehaviour
{
    [SerializeField] private ParticleSystem HealthPickupEffect;
    public AudioSource powerupSound;
    public GameObject parent;
    public float duration = 5f;

    public void Start()
    {
        powerupSound = GetComponent<AudioSource>();
        parent = transform.parent.gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(SpeedPickup(other));
        }
    }

    IEnumerator SpeedPickup(Collider player)
    {
        PlayerMovement move = player.GetComponentInParent<PlayerMovement>();
        PlayerDamageable stats = player.GetComponent<PlayerDamageable>();
        powerupSound.Play();
        foreach (Transform child in parent.gameObject.transform)
        {
            child.GetComponent<MeshRenderer>().enabled = false;
        }
        transform.parent.position = new Vector3(0, -100, 0);
        stats.isSpeed = true;

        float time = 0;
        float origSpeed;
        float newSpeed;
        if (!move.hasSpeedPowerup) {
            move.hasSpeedPowerup = true;
            while (time <= duration) {
                origSpeed = move.getTargetSpeed(move.playerState);
                newSpeed = origSpeed * 1.5f;

                move.powerUpSpeed = newSpeed;
                time += Time.deltaTime;
                yield return null;
            }
            move.hasSpeedPowerup = false;
        }

        yield return new WaitForSeconds(1);
        stats.isSpeed = false;
        Destroy(transform.parent.gameObject);
        
        yield return null;
    }
}
