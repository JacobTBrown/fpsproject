using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Zach/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private string _gameVersion = "0.0.0";

    public string GameVersion {  get { return _gameVersion; } }
    public string _nickName = "puntestname";

    public string NickName
    {
        get
        {
            int value = Random.Range(0, 999);
            return _nickName + value.ToString();
        }

    }
}
