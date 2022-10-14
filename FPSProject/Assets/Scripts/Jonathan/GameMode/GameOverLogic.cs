using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Scripts.Jonathan;

public class GameOverLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        EventManager.AddListener<EndGameEvent>(onGameOver);
    }

    public void onGameOver(EndGameEvent evt){
        gameObject.SetActive(true);
    }
}