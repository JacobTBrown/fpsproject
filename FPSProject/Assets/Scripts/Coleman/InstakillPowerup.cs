using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstakillPowerup : MonoBehaviour
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
            StartCoroutine(KillPowerup(other));
        }
    }

    IEnumerator KillPowerup(Collider player)
    {
        PlayerDamageable stats = player.GetComponent<PlayerDamageable>();
        Gun[] gun = player.transform.parent.GetComponentsInChildren<Gun>();
        powerupSound.Play();
        foreach (Transform child in parent.gameObject.transform)
        {
            child.GetComponent<MeshRenderer>().enabled = false;
        }
        stats.isInstakill = true;

        transform.parent.position = new Vector3(0, -100, 0);
        for (int i = 0; i < gun.Length; i++) {
            gun[i].isInstakill = true;
        }

        float time = 0;
        while (time <= duration) {
            time += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < gun.Length; i++) {
            gun[i].isInstakill = false;
        }

        yield return new WaitForSeconds(1);
        stats.isInstakill = false;
        Destroy(transform.parent.gameObject);
        
        yield return null;
    }
}
