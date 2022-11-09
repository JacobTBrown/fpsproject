using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorTextFade : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(fadeOut());
    }

    private IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(5f);
        gameObject.GetComponent<Text>().text = "";
    }
}
