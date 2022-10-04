using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public PhotonView PV;
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    public void SetHealth(float health, PhotonView Photon)
    {
        PV = Photon;
        if (PV.IsMine)
        {
            slider.value = health;
        }
    }
}
