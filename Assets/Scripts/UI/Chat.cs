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

    void ExpandContent(int lineCount)
    {
        Vector2 size = OutputText.GetComponent<RectTransform>().sizeDelta;
        size.y = OutputText.fontSize * lineCount;
        OutputText.GetComponent<RectTransform>().sizeDelta = size;
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
        AddLine("서버에 연결되었습니다.");

        ChannelName = PhotonNetwork.CurrentRoom.Name + "_" + PhotonNetwork.CurrentRoom.MasterClientId.ToString();
        ChatClient.Subscribe(new string[] { ChannelName }, 10);
    }

    public void OnDisconnected()
    {
        AddLine("서버와 연결이 끊어졌습니다.");
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("OnChatStateChange = " + state);
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        AddLine(string.Format("채널 입장 ({0})", string.Join(",", channels)));
    }

    public void OnUnsubscribed(string[] channels)
    {
        AddLine(string.Format("채널 퇴장 ({0})", string.Join(",", channels)));
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

        if(channel.MessageCount > 5)
            ExpandContent(channel.MessageCount * 2);
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
