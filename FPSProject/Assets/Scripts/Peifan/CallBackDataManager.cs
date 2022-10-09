using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CallBackDataManager : MonoBehaviour
{

    //public InputField UserId;

    public void ClickDown(TMP_InputField UserId)
    {
        DataManager.Instance.SetUserID(UserId.text);
    }
}
