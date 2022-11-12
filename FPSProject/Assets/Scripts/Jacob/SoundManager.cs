using Photon.Pun;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] footsteps;
    public AudioSource footstepSource;
    private PhotonView PV;
    private Rigidbody playerRigidbody;
    private PlayerMovement playerMovement;
    private int routineCount = 0;
    private IEnumerator routine;
    private int current;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        playerRigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        
        routineCount = 0;
        current = 0;
    }

    void Update() {
        if (PV.IsMine) {
            UpdateRoutine();
        } else { 
            UpdateRoutine();
        }
    }

    public void UpdateRoutine() {
        if (playerMovement.playerState == PlayerMovement.MovementState.walking
            || playerMovement.playerState == PlayerMovement.MovementState.sprinting) {
                if (routineCount == 0) {
                    routine = ChooseAFootstep();
                    StartCoroutine(routine);
                    routineCount++;   
                }
            } else {
                if (routineCount == 1 && routine != null) {
                    StopCoroutine(routine);
                    routineCount = 0;
                } 
            }
        // if (playParkourSound) {
        //     StartCoroutine(Parkour());
        // }
    }

    public IEnumerator ChooseAFootstep() { 
        while(true) {
            if (current > 9) current = 0;
            float vel = playerRigidbody.velocity.magnitude;

            if (playerMovement.playerState == PlayerMovement.MovementState.slowwalking
                && playerMovement.playerState == PlayerMovement.MovementState.inAir)
                break;

            if (playerMovement.isOnGround && vel > 2f)
            {
                footstepSource.clip = footsteps[current];
                if (playerMovement.playerState == PlayerMovement.MovementState.walking) {
                    footstepSource.volume = 0.10f;
                    footstepSource.PlayOneShot(footstepSource.clip);

                    float interval = footstepSource.clip.length;
                    current++;
                    yield return new WaitForSeconds(interval + 0.20f);
                } else if (playerMovement.playerState == PlayerMovement.MovementState.sprinting) {
                    footstepSource.volume = 0.28f;
                    footstepSource.PlayOneShot(footstepSource.clip);

                    float interval = footstepSource.clip.length;
                    current++;
                    yield return new WaitForSeconds(interval);
                }
            }
            else
                yield return 0;
        }
    }
}
