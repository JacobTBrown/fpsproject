using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillHUD : MonoBehaviour
{
    public TextMeshProUGUI killText;

    private void Awake()
    {
        killText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void updateText(string text)
    {
        killText.text = text;
    }
}
