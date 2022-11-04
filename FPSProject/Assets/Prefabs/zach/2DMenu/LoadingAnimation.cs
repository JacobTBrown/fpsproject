using DG.Tweening;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField] GameObject square;
    private int speed = 1;

    void Awake()
    {
        square.SetActive(true);
        StartCoroutine(SpinRoutine());
        
    }

    private IEnumerator SpinRoutine()
    {
        yield return new WaitForSeconds(.1f);
        transform.DORotate(new Vector3(1, 10, 0), 1).SetEase(Ease.OutCirc).SetLoops(-1, LoopType.Yoyo);
        //transform.DOMove(new Vector3(500,0, 0), 1).SetEase(Ease.OutCirc).SetLoops(-1, LoopType.Yoyo);
        while (this.gameObject.activeInHierarchy)

        {

          //  i++;
            //square.transform.Rotate(0, 0, speed);
            //square.transform.localScale = new Vector3(square.transform.localScale.x + speed/4000000, square.transform.localScale.y + speed/4000000, square.transform.localScale.z);
            //if (i == 10) { i = 0; speed *= -1; }
            yield return new WaitForSeconds(1f);
        }
    }
    
}
