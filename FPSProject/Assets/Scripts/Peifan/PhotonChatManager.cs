using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Author: Peifan Tian
    Creation: 10/03/22
    Last Edit: 11/03/22 -Peifan

*/
public class PhotonChatManager : MonoBehaviour, IChatClientListener
{


    private bool isConnected;
    public string playerName;
    public GameObject joinChatButton;
    private ChatClient chatClient;
    public static PhotonChatManager instance;
    public Dropdown m_ddown;
    public PhotonView PV;

    private void Awake()
    {
        instance = this;
        PV = gameObject.GetComponent<PhotonView>();
        //DontDestroyOnLoad(this);
    }
    public void UsernameOnValueChange(string valueInput)
    {
        playerName = valueInput;
    }

    public void ChatConnectOnClick()
    {
        isConnected = true;
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(playerName));
        //chatClient.ConnectAndSetStatus(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(playerName), ChatUserStatus.Playing);
    }

    public GameObject chatPanel;
    public InputField chatField;
    public Text chatDisplay;
    private string sendPrivatelyTo = "";
    private string enterText;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("Delay", 2f);
    }

    void Delay()
    {
        chatClient.PublishMessage(DataManager.Instance.GetRoomName(), m_ddown.value.ToString() + "--joingame");
        //chatClient.PublishMessage(DataManager.Instance.GetRoomName(),"joingame");
        bool isSubscribed = chatClient.Subscribe(DataManager.Instance.GetRoomName());
        if (isSubscribed)
        {
            //Debug.LogError("Subscribed");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isConnected == true)
        {
            chatClient.Service();
        }

        if (chatField.text != "")
        {
            if (Input.GetKey(KeyCode.Return))
            {
                SubmitPublicChatOnClick();
                SubmitPrivateChatOnClick();
            }
        }
    }

    public void SubmitPublicChatOnClick()
    {
        if (sendPrivatelyTo == "")
        {
            if (enterText == "") return;
            string InputTxt = m_ddown.value.ToString() + "--" + enterText;
            //Debug.LogError("DataManager.Instance.GetRoomName() = " + DataManager.Instance.GetRoomName());
            //chatClient.PublishMessage("RegionChannel", enterText);
            chatClient.PublishMessage(DataManager.Instance.GetRoomName(), InputTxt);
            chatField.text = "";
            enterText = "";
        }
    }

    public void TypeChatOnValueChange(string valueInput)
    {
        enterText = valueInput;
    }

    public void ReceiverOnValueChange(string valueInput)
    {
        sendPrivatelyTo = valueInput;
    }

    public void SubmitPrivateChatOnClick()
    {
        if (sendPrivatelyTo != "")
        {
            if (enterText == "") return;
            string InputTxt = m_ddown.value.ToString() + "--" + enterText;
            chatClient.SendPrivateMessage(sendPrivatelyTo, InputTxt);
            chatField.text = "";
            enterText = "";
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        if (state == ChatState.Uninitialized)
        {
            isConnected = false;
            joinChatButton.SetActive(true);
            chatPanel.SetActive(false);
        }

    }

    public void OnConnected()
    {
        joinChatButton.SetActive(false);
        chatClient.Subscribe(new string[] { "RegionChannel" });
    }

    public void OnDisconnected()
    {
        isConnected = false;
        joinChatButton.SetActive(true);
        chatPanel.SetActive(false);
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName == "World") return;
        string text = "";
        int length = 0;
        for (int i = 0; i < senders.Length; i++)
        {
            if (senders[i] != playerName) 
            {
                string[] value = messages[i].ToString().Split(new string[] { "--" }, System.StringSplitOptions.None);
                if (value.Length < 2) return;
                if (value[1] == "AddMoveSpeed") return;
                if (value[1] == "ReduceMoveSpeed") return;
                if (value[1] == "AddJump") return;
                if (value[1] == "ReduceJump") return;
                if (value[1] == "AddAirSpeed") return;
                if (value[1] == "ReduceAirSpeed") return;
            }
        }

        while (length < senders.Length) {
            text = senders[length] + ": " + messages[length];
            string[] value = text.Split(new string[] { "--" }, System.StringSplitOptions.None);
               if (value.Length < 2)
            {
                return;
            }
            if (value.Length < 2) return;
            string newtext = MakeText(value[0], value[1]);
            if (newtext != null) 
            {
                chatDisplay.text += "\n" + newtext;
                length++;
            }
        }

    }

    public Action AddMoveSpeedEvent;
    public string AddMoveSpeed() 
    {
        //Debug.LogError("SpeedUp");
        AddMoveSpeedEvent?.Invoke();
        //PV.RPC("AddSpeed", RpcTarget.AllBuffered, "MoveSpeed", 10);
        return "SpeedUp";
    }
    public Action ReduceMoveSpeedEvent;
    public string ReduceMoveSpeed()
    {
        //Debug.LogError("SpeedDown");
        ReduceMoveSpeedEvent?.Invoke();
        //PV.RPC("SubtractSpeed", RpcTarget.AllBuffered, "MoveSpeed", 2);
        return "SpeedDown";
    }
    public Action AddJumpEvent;
    public string AddJump()
    {
        //Debug.LogError("JumpUp");
        AddJumpEvent?.Invoke();
        //PV.RPC("AddSpeed", RpcTarget.AllBuffered, "JumpSpeed", 10);
        return "JumpUp";
    }
    public Action ReduceJumpEvent;
    public string ReduceJump()
    {
        //Debug.LogError("JumpDown");
        ReduceJumpEvent?.Invoke();
        //PV.RPC("SubtractSpeed", RpcTarget.AllBuffered, "JumpSpeed", 5);
        return "JumpDown";
    }
    public Action AddAirSpeedEvent;
    public string AddAirSpeed()
    {
        //Debug.LogError("AirSpeedUp");
        AddAirSpeedEvent?.Invoke();
        //PV.RPC("AddSpeed", RpcTarget.AllBuffered, "AirSpeed", 10);
        return "AirSpeedUp";
    }
    public Action ReduceAirSpeedEvent;
    public string ReduceAirSpeed()
    {
        //Debug.LogError("AirSpeedDown");
        ReduceAirSpeedEvent?.Invoke();
        //PV.RPC("SubtractSpeed", RpcTarget.AllBuffered, "AirSpeed", 5);
        return "AirSpeedDown";
    }

    public string MakeText(string style, string value)
    {
        string retvalue = "";
        //Debug.LogError("style = " + style);
        string[] tmp = style.Split(new string[] { ":" }, System.StringSplitOptions.None);

        if (value == "AddMoveSpeed") value = AddMoveSpeed();
        else if (value == "ReduceMoveSpeed") value = ReduceMoveSpeed();
        else if (value == "AddJump") value = AddJump();
        else if (value == "ReduceJump") value = ReduceJump();
        else if (value == "AddAirSpeed") value = AddAirSpeed();
        else if (value == "ReduceAirSpeed") value = ReduceAirSpeed();

        switch (tmp[1].Trim())
        {
            case "0":
                retvalue = tmp[0] + ":" + value;
                break;
            case "1":
                retvalue = "<Color=red><Size=160>" + tmp[0] + ":" + value + "</Size></Color>";
                break;
            case "2":
                retvalue = "<Color=blue><Size=140>" + tmp[0] + ":" + value + "</Size></Color>";
                break;
            case "3":
                retvalue = "<Color=green><Size=130>" + tmp[0] + ":" + value + "</Size></Color>";
                break;
            case "4":
                retvalue = "<Color=yellow><Size=150>" + tmp[0] + ":" + value + "</Size></Color>";
                break;
            default:
                break;
        }
        return retvalue;
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        string text = "(private)" + " " + sendPrivatelyTo +": "+ message;
        string[] value = text.Split(new string[] { "--" }, System.StringSplitOptions.None);
        string newtext = MakeText(value[0], value[1]);
        chatDisplay.text +=  "\n " + newtext;

    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        chatPanel.SetActive(true);
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

}
