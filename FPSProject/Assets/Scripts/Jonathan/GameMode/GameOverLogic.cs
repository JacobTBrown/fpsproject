using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Scripts.Jonathan;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

public class GameOverLogic : MonoBehaviour
{
    public Image endScreen;
    private Tween fadeTween;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddListener<EndGameEvent>(onGameOver);
        gameObject.SetActive(false);
        endScreen = gameObject.GetComponentInParent<Image>();    
        endScreen.gameObject.SetActive(false);        
    }

    public void onGameOver(EndGameEvent evt){
        gameObject.SetActive(true);
        gameObject.transform.parent.gameObject.SetActive(true);
        FadeInFadeOut(1.0f, 1.0f);
        Invoke("ClearEndOfGame", 10f);
        EventManager.RemoveListener<EndGameEvent>(onGameOver);
    }
    
    private void FadeInFadeOut(float endValue, float duration)
    {
        endScreen.DOFade(255, duration).OnComplete(() => {
            endScreen.DOFade(0, duration);
        });
    }

    public void ClearEndOfGame()
    {
        gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
        //exit button 
    }
}