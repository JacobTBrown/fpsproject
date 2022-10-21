using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RPC_Functions : MonoBehaviourPunCallbacks
{
    private Animator animator;
    public AudioSource gunShot;
    public AudioSource reload;

    public void Start() {
        animator = GetComponentInChildren<Animator>();
        Debug.Log(animator.name + " was animator");
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
    [PunRPC]
    public void ClearRPCs(Player o)
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " cleared rpcs for " + o.ActorNumber);
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.RemoveRPCs(o);
    }
}
