using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Zach/MasterManager")]
public class MasterManager : SingletonScriptableObject<MasterManager>
{

    
    [SerializeField]
    private SpeedManager _speedManager;

    [SerializeField]
    private GameSettings _gameSettings;
    //public static GameSettings GameSettings { get { return instance.GameSettings; } }
    public static SpeedManager speedManager { get { return instance._speedManager;  } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        //Debug.Log("Before awake");

    }
    private static void Awake()
    {
       // Instance = this;
    }
   
}
