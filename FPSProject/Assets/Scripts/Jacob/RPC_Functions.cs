using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPC_Functions : MonoBehaviour
{
    private Animator animator;
    public AudioSource gunShot;
    public AudioSource reload;

    public void Start() {
        animator = GetComponentInChildren<Animator>();
    }

    [PunRPC]
    public void triggerAnim(string s) {
        animator.SetTrigger(s);
        if (s == "Reload") {
            reload.Play();
        } else if (s == "Shoot") {
            gunShot.Play();
        }
    }
}
