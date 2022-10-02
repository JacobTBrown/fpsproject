using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] footsteps;
    private AudioSource audioSource;
    private Rigidbody playerRigidbody;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();

        StartCoroutine(ChooseAFootstep());
    }

    public IEnumerator ChooseAFootstep() {
        int i = 0;
        while(true) {
            if (i > 9) i = 0;
            float vel = playerRigidbody.velocity.magnitude;
            if (playerMovement.playerState != PlayerMovement.MovementState.slowwalking && playerMovement.isOnGround && vel > 2f)
            {
                audioSource.clip = footsteps[i];
                if (playerMovement.playerState == PlayerMovement.MovementState.walking) {
                    audioSource.volume = 0.10f;
                    audioSource.PlayOneShot(audioSource.clip);
                    float interval = audioSource.clip.length;
                    i++;
                    yield return new WaitForSeconds(interval + 0.20f);
                } else if (playerMovement.playerState == PlayerMovement.MovementState.sprinting) {
                    audioSource.volume = 0.28f;
                    audioSource.PlayOneShot(audioSource.clip);
                    float interval = audioSource.clip.length;
                    i++;
                    yield return new WaitForSeconds(interval);
                }
            }
            else
                yield return 0;
        }

    }
}
