using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonNoGameChatManager : MonoBehaviour, IChatClientListener
{
    private bool isConnected;
    public string playerName;
    public GameObject obj;
    public GameObject joinChatButton;
    private ChatClient chatClient;
    public static PhotonNoGameChatManager instance;
    public Dropdown m_ddown;

    private void Awake()
    {
        instance = this;
    }

    public void StartChat()
    {
        obj.gameObject.SetActive(true);
        playerName = DataManager.Instance.UserId;
        isConnected = true;
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(playerName));
        Invoke("Delay", 2f);
    }
    void Delay()
    {
        chatClient.PublishMessage("World", "joindating");
        bool isSubscribed = chatClient.Subscribe("World");
        if (isSubscribed)
        {
            //Debug.LogError("Subscribed");
        }

    }
    public GameObject chatPanel;
    public InputField chatField;
    public Text chatDisplay;
    private string sendPrivatelyTo = "";
    private string enterText;

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
            //chatClient.PublishMessage("RegionChannel", InputTxt); 
            chatClient.PublishMessage("World", InputTxt);
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
        string text = "";
        int length = 0;

        while (length < senders.Length)
        {
            text = senders[length] + ": " + messages[length];
            string[] value = text.Split(new string[] { "--"},System.StringSplitOptions.None);
            string newtext = MakeText(value[0], value[1]);
            chatDisplay.text += "\n" + newtext;
            length++;
        }

    }


    public string MakeText(string style,string value) 
    {
        string retvalue = "";
        //Debug.LogError("style = " + style);
        string[] tmp = style.Split(new string[] { ":"},System.StringSplitOptions.None);
        //Debug.LogError("tmp[1] = " + tmp[1]);
        switch (tmp[1].Trim())
        {
            case "0":
                retvalue = tmp[0] + ":" + value;
                break;
            case "1":
                retvalue = "<Color=red><Size=20>" + tmp[0] + ":" + value + "</Size></Color>";
                break;
            case "2":
                retvalue = "<Color=blue><Size=16>" + tmp[0] + ":" + value + "</Size></Color>";
                break;
            case "3":
                retvalue = "<Color=green><Size=18>" + tmp[0] + ":" + value + "</Size></Color>";
                break;
            case "4":
                retvalue = "<Color=yellow><Size=20>" + tmp[0] + ":" + value + "</Size></Color>";
                break;
            default:
                break;
        }
        return retvalue;
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        string text = "(private)" + " " + sendPrivatelyTo + ": " + message;
        string[] value = text.Split(new string[] { "--" }, System.StringSplitOptions.None);
        string newtext = MakeText(value[0], value[1]);
        chatDisplay.text += "\n " + newtext;

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
