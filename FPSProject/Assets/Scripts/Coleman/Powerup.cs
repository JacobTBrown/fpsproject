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
            StartCoroutine(Pickup());
        }
    }

    IEnumerator Pickup()
    {
        pickupEffect.Play();
        powerupSound.Play();
        foreach (Transform child in parent.gameObject.transform)
        {
            child.GetComponent<MeshRenderer>().enabled = false;
        }
        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(3);
        Destroy(transform.parent.gameObject);
    }
}
