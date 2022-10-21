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
    public void Update()
    {
        if (!animator)
        {
            Debug.Log("animator was null in RPC_Functions.cs");
        }
    }
    [PunRPC]
    public void triggerAnim(string s) {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        if (s.Length == 0)
        {
            Debug.Log("bad animation call");
            return;
        }
        Debug.Log("triggered anim " + s);
        Debug.Log("animator ws " + animator.name + " reload was " + reload.name);
        animator.SetTrigger(s);
        if (s == "Reload") {
            reload.Play();
        } else if (s == "Shoot") {
            gunShot.Play();
        }
    }
}
