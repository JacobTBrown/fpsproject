using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatUserNameInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonChatManager.instance.UsernameOnValueChange(DataManager.Instance.GetUserID());
        PhotonChatManager.instance.ChatConnectOnClick();
    }


}
