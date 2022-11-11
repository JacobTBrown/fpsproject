using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//error text script: 
// Pass it a string and i'll auto-clear it after a few seconds
//-zach 11/10

public class ErrorTextFade : MonoBehaviour
{
    bool textActive;
    public void FourSecFade(string text)
    {

        if (!textActive) { // if the text is already active, do nothing
        gameObject.GetComponent<Text>().text = text;
            textActive = true;
        StartCoroutine(fadeOut());
        }
        else
        {
            //play error sound?
        }
    }

    private IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(4f);
        gameObject.GetComponent<Text>().text = "";
        textActive = false;
    }
}
