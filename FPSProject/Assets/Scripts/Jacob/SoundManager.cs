using Photon.Pun;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour, IPunObservable
{
    public AudioClip[] footsteps;
    private AudioSource audioSource;
    private PhotonView PV;
    private Rigidbody playerRigidbody;
    private PlayerMovement playerMovement;
    private int routineCount = 0;
    private IEnumerator routine, otherRoutine;
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

    public void OnPhotonSerializeView(PhotonStream s, PhotonMessageInfo i) {
        if (s.IsWriting) {
            //Debug.Log("writing from soundmanager");
            s.SendNext(current);
            if (playerRigidbody != null)
                s.SendNext(playerRigidbody.velocity);
            if (playerMovement != null) {
                s.SendNext(playerMovement.isOnGround);
                s.SendNext(playerMovement.playerState);
            }
        } else {
            //Debug.Log("reading in soundmanager");
            current = (int) s.ReceiveNext();
            if (playerRigidbody != null) {
                Vector3 temp = (Vector3)s.ReceiveNext();
                //Debug.Log("Recieve next from SoundManager.cs: " + temp.GetType()) ;
                playerRigidbody.velocity = temp;
                //playerRigidbody.velocity = (Vector3)s.ReceiveNext();
            }
            if (playerMovement != null) {
                playerMovement.isOnGround = (bool) s.ReceiveNext();
                playerMovement.playerState = (PlayerMovement.MovementState) s.ReceiveNext();
            }
        }
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
