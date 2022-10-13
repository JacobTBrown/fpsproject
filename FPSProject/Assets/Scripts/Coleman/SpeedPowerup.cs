using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerup : MonoBehaviour
{
    [SerializeField] private ParticleSystem HealthPickupEffect;
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
            StartCoroutine(SpeedPickup(other));
        }
    }

    IEnumerator SpeedPickup(Collider player)
    {
        PlayerMovement move = player.GetComponentInParent<PlayerMovement>();
        powerupSound.Play();
        foreach (Transform child in parent.gameObject.transform)
        {
            child.GetComponent<MeshRenderer>().enabled = false;
        }
        GetComponent<Collider>().enabled = false;

        float duration = 5f;
        float time = 0;
        float origSpeed;
        float newSpeed;
        if (!move.hasSpeedPowerup) {
            move.hasSpeedPowerup = true;
            while (time <= duration) {
                origSpeed = move.getMoveSpeed(move.playerState);
                newSpeed = origSpeed * 1.5f;

                move.powerUpSpeed = newSpeed;
                time += Time.deltaTime;
                yield return null;
            }
            move.hasSpeedPowerup = false;
        }
        Destroy(transform.parent.gameObject);
        
        yield return null;
    }
}
