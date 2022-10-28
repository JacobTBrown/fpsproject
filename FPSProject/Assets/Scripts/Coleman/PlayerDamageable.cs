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
        Debug.Log("Starting player damage");
        impact = GetComponent<AudioSource>();
        DamageFlash = GameObject.Find("DamageFlash").GetComponent<Animator>();
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        Debug.Log("Healthbar is: " + healthBar.name);
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;

        //healthBar.SetMaxHealth(100);
        Debug.Log(currentHealth);
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
<<<<<<< HEAD
           // onDie(PV.ViewID);
=======
            onDie(PV.ViewID);
>>>>>>> Jonathan

        }
    }

    public void Damage(float damage, int EnemyPlayer)
    {
        if(PV.IsMine)
        {
            if (!isInvincible)
            {
                currentHealth -= damage;
                impact.Play();
                DamageFlash.SetTrigger("Damage");
                healthBar.SetHealth(currentHealth, PV);
                Debug.Log("Hit Player for " + damage + " damage. Player is now at " + currentHealth + " HP.");
            }
        }
        if (currentHealth <= 0)
        {
            onDie(EnemyPlayer);
<<<<<<< HEAD
            PlayerDeathEvent evt = Events.PlayerDeathEvent; // ?
=======
            PlayerDeathEvent evt = Events.PlayerDeathEvent;
>>>>>>> Jonathan
            if (PV.IsMine)
            {
                evt.player = player;
                currentHealth = 100;
                healthBar.SetHealth(currentHealth, PV);
               //EventManager.Broadcast(evt);
            }
            
            Debug.Log("A player has died!");
            
            //Destroy(transform.gameObject);
            //chaging to jonathan's script
        }
    }

    public void onDie(int EnemyPlayer){
<<<<<<< HEAD
     
            Debug.Log(PV.ViewID + " was killed by " + EnemyPlayer);
            RaiseEventOptions o = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PlayerStatsPage pstats = GameObject.Find("RoomManager").GetComponent<PlayerStatsPage>();
            pstats.gotKill = EnemyPlayer;
            pstats.onDie = true;
            pstats.gotKilled = PV.ViewID;
            int DeadviewID = PV.ViewID;
            //object[] obj = new object[]{DeadviewID, EnemyPlayer};
            object[] obj = {DeadviewID, EnemyPlayer};
        //Debug.Log(obj[0].ToString() + obj[1].ToString() + " was your obj");
            PhotonNetwork.RaiseEvent(0, obj, o, SendOptions.SendReliable); //PhotonEvent.PLAYERDEATH
            //PhotonNetwork.RaiseEvent(PhotonEvents.PLAYERKILL, obj, o, SendOptions.SendReliable);
=======
        if(PV.IsMine)
        {
            RaiseEventOptions o = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            int DeadviewID = PV.ViewID;
    
            object[] obj = new object[]{DeadviewID,EnemyPlayer};
            PhotonNetwork.RaiseEvent(PhotonEvents.PLAYERDEATH,obj,o,SendOptions.SendReliable);
>>>>>>> Jonathan
        }
}
