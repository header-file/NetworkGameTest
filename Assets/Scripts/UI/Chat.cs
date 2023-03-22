using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using Photon.Pun;

public class Chat : MonoBehaviour, IChatClientListener
{
    public InputField InputField;
    public Text OutputText;
    public GameObject ScrollBar;

    ChatClient ChatClient;
    string Nickname;
    string ChannelName;


    void Start()
    {
        Application.runInBackground = true;
        Nickname = PhotonNetwork.NickName;

        ChatClient = new ChatClient(this);
        ChatClient.Connect("", "1.0", new AuthenticationValues(PhotonNetwork.LocalPlayer.NickName));
        ScrollBar.SetActive(false);
    }

    void Update()
    {
        ChatClient.Service();

        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            OnClickPublishMessage();
    }

    void ExpandContent()
    {
        RectTransform rect = OutputText.GetComponent<RectTransform>();
        Vector2 size = rect.sizeDelta;
        size.y = OutputText.preferredHeight;
        rect.sizeDelta = size;
        Vector2 pos = rect.anchoredPosition;
        pos.y = size.y;
        rect.anchoredPosition = pos;
    }

    public void AddLine(string stringLine)
    {
        OutputText.text += stringLine + "\r\n";
    }

    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    public void OnConnected()
    {
        AddLine("������ ����Ǿ����ϴ�.");

        ChannelName = PhotonNetwork.CurrentRoom.Name + "_" + PhotonNetwork.CurrentRoom.MasterClientId.ToString();
        ChatClient.Subscribe(new string[] { ChannelName }, 10);
    }

    public void OnDisconnected()
    {
        AddLine("������ ������ ���������ϴ�.");
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("OnChatStateChange = " + state);
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        AddLine(string.Format("ä�� ���� ({0})", string.Join(",", channels)));
    }

    public void OnUnsubscribed(string[] channels)
    {
        AddLine(string.Format("ä�� ���� ({0})", string.Join(",", channels)));
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (!ChannelName.Equals(channelName))
            return;

        ShowChannel(channelName);
    }

    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
            return;

        ChatChannel channel = null;
        bool found = ChatClient.TryGetChannel(channelName, out channel);
        if (!found)
        {
            Debug.Log("ShowChannel failed to find channel: " + channelName);
            return;
        }

        ChannelName = channelName;
        OutputText.text = channel.ToStringMessages();

        if(OutputText.preferredHeight > 350.0f)
            ExpandContent();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("OnPrivateMessage : " + message);
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log("status : " + string.Format("{0} is {1}, Msg : {2} ", user, status, message));
    }

    public void OnUserSubscribed(string channel, string user)
    {
        AddLine("[" + channel + "] " + user);
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        AddLine("[" + channel + "] " + user);
    }

    public void OnClickPublishMessage()
    {
        if (InputField.text != "" &&
            ChatClient.State == ChatState.ConnectedToFrontEnd)
        {
            ChatClient.PublishMessage(ChannelName, InputField.text);

            InputField.text = "";
        }
    }

    public void OnPressOutputText()
    {
        ScrollBar.SetActive(true);
    }

    public void OnReleaseOutputText()
    {
        ScrollBar.SetActive(false);
    }

    void OnApplicationQuit()
    {
        if (ChatClient != null)
            ChatClient.Disconnect();
    }
}
