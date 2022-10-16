using Photon.Pun;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour//, IPunObservable
{
    public AudioClip[] footsteps;
    private AudioSource audioSource;
    private PhotonView PV;
    private Rigidbody playerRigidbody;
    private PlayerMovement playerMovement;
    private int routineCount;
    private IEnumerator routine;
    private int current;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        PV = GetComponent<PhotonView>();
        routineCount = 0;
        current = 0;
    }

    // public void OnPhotonSerializeView(PhotonStream s, PhotonMessageInfo i) {
    //     if (s.IsWriting) {
    //         Debug.Log("writing from soundmanager");
    //         s.SendNext(footsteps[current]);
    //     } else {
    //         Debug.Log("reading in soundmanager");
    //         //s.ReceiveNext
    //     }
    // }

    void Update() {
        if (PV.IsMine) {
            if (playerMovement.playerState == PlayerMovement.MovementState.walking
            || playerMovement.playerState == PlayerMovement.MovementState.sprinting) {
                if (routineCount == 0)
                {
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
        }
    }

    public IEnumerator ChooseAFootstep() {
        while(true) {
            if (current > 9) current = 0;
            float vel = playerRigidbody.velocity.magnitude;
            if (playerMovement.playerState != PlayerMovement.MovementState.slowwalking && playerMovement.isOnGround && vel > 2f)
            {
                audioSource.clip = footsteps[current];
                if (playerMovement.playerState == PlayerMovement.MovementState.walking) {
                    audioSource.volume = 0.10f;
                    audioSource.PlayOneShot(audioSource.clip);

                    float interval = audioSource.clip.length;
                    current++;
                    yield return new WaitForSeconds(interval + 0.20f);
                } else if (playerMovement.playerState == PlayerMovement.MovementState.sprinting) {
                    audioSource.volume = 0.28f;
                    audioSource.PlayOneShot(audioSource.clip);

                    float interval = audioSource.clip.length;
                    current++;
                    yield return new WaitForSeconds(interval);
                }
            }
            else
                yield return 0;
        }

    }
}
