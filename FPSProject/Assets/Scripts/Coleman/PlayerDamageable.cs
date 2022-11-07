using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Scripts.Jonathan;
using UnityEngine;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] public float currentHealth;
    public float maxHealth = 100f;
    public bool isInvincible = false;
    public bool isSpeed = false;
    public HealthBar healthBar;
    public Animator DamageFlash;
    public GameObject player;
    public PhotonView PV;
    AudioSource impact;

    void Awake()
    {
        player = transform.parent.gameObject;
        PV = player.GetComponent<PhotonView>();
    }
    void Start()
    {
        impact = GetComponent<AudioSource>();
        DamageFlash = GameObject.Find("DamageFlash").GetComponent<Animator>();
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
        //Debug.Log(currentHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Debug.Log("TEST DAMAGE KEY PRESSED, PLAYER TAKES 20 DAMAGE!");
            Damage(20f, PV.ViewID);
        }
        healthBar.SetHealth(currentHealth, PV);
        if (isInvincible) healthBar.changeColor(PV, Color.blue);
        else if (isSpeed) healthBar.changeColor(PV, Color.green);
        else healthBar.changeColor(PV, Color.red);

        if (Input.GetKeyDown(KeyCode.K))
        {
            /*
                This is for testing purposes only
            */
            onDie(PV.ViewID);
        }
    }

    public void Damage(float damage, int EnemyPlayer)
    {
        PlayerStatsPage pstats = GameObject.Find("RoomManager").GetComponent<PlayerStatsPage>();
        PhotonView enemyPV = PhotonView.Find(EnemyPlayer);
        //Debug.Log("began damage");
    /*    if (pstats.CheckTeam(enemyPV)) // if we're on the same team, just return
        {
            return;
        }*/
        if(PV.IsMine)
        {
            if (!isInvincible)
            {
                currentHealth -= damage;
                impact.Play();
                DamageFlash.SetTrigger("Damage");
                healthBar.SetHealth(currentHealth, PV);
                //Debug.Log("Hit Player for " + damage + " damage. Player is now at " + currentHealth + " HP.");
            }
        }
        if (currentHealth <= 0)
        {
            onDie(EnemyPlayer);
            PlayerDeathEvent evt = Events.PlayerDeathEvent;  
            if (PV.IsMine)
            {
                evt.player = player;
                currentHealth = 100;
                healthBar.SetHealth(currentHealth, PV);
            }         
        
        }
    }

    public void onDie(int EnemyPlayer){
     
            //Debug.Log(PV.ViewID + " was killed by " + EnemyPlayer);
            RaiseEventOptions o = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            int DeadviewID = PV.ViewID;
            object[] obj = {DeadviewID, EnemyPlayer};
            PhotonNetwork.RaiseEvent(0, obj, o, SendOptions.SendReliable); //PhotonEvent.PLAYERDEATH

        }
}
