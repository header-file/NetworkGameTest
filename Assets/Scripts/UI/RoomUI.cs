using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomUI : MonoBehaviour
{
    public Transform PlayerSlots;
    public Text RoomName;
    public GameObject ReadyBtn;
    public GameObject StartBtn;
    public GameObject AssertionExit;

    public PlayerSlot[] PSlots;

    Hashtable RoomPlayerProperties;


    void Awake()
    {
        RoomPlayerProperties = new Hashtable();
        RoomPlayerProperties.Add("IsReady", false);
        RoomPlayerProperties.Add("IsStart", false);
        RoomPlayerProperties.Add("Index", 0);

        AssertionExit.SetActive(false);
        gameObject.SetActive(false);
    }

    void Update()
    {
        PlayerUpdate();
        BtnUpdate();
        CheckStart();
    }

    void PlayerUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                if (i < PhotonNetwork.PlayerList.Length)
                {
                    if (i == PhotonNetwork.PlayerList[i].ActorNumber - 1)
                    {
                        if (PSlots[i].Empty.activeSelf)
                            PSlots[i].PlayerOn(PhotonNetwork.PlayerList[i].NickName);

                        if (PhotonNetwork.PlayerList[i].CustomProperties.ContainsKey("IsReady") != false &&
                            PhotonNetwork.PlayerList[i].CustomProperties.GetValueOrDefault("IsReady").Equals(true))
                            PSlots[i].Ready.SetActive(true);
                        else
                            PSlots[i].Ready.SetActive(false);
                    }
                }
                else
                    PSlots[i].PlayerOff();
            }
        }
    }

    void BtnUpdate()
    {
        if (!StartBtn.activeSelf)
            return;

        int readyCount = 0;
        for(int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if (PhotonNetwork.PlayerList[i].CustomProperties.ContainsKey("IsReady") != false &&
                PhotonNetwork.PlayerList[i].CustomProperties.GetValueOrDefault("IsReady").Equals(true))
                readyCount++;
        }

        if (readyCount > 1 && 
            readyCount == PhotonNetwork.CurrentRoom.PlayerCount)
            StartBtn.GetComponent<Button>().interactable = true;
        else
            StartBtn.GetComponent<Button>().interactable = false;
    }

    void CheckStart()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("IsStart") != false &&
            PhotonNetwork.LocalPlayer.CustomProperties.GetValueOrDefault("IsStart").Equals(true))
        {
            //PhotonNetwork.LocalPlayer.CustomProperties.Clear();

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                if(PhotonNetwork.PlayerList[i].UserId == PhotonNetwork.LocalPlayer.UserId)
                   PhotonNetwork.PlayerList[i].CustomProperties["Index"] = i;

            GameManager.Inst().PlayerCount = PhotonNetwork.PlayerList.Length;
            GameManager.Inst().SpanwPlayer();
            GameManager.Inst().TurnManager.NextTurn();
            GameManager.Inst().TurnManager.DiceBox.Reroll();

            GameManager.Inst().UiManager.CloseLobbyAndRoom();
        }
    }

    public void CreateRoom()
    {
        RoomPlayerProperties["IsReady"] = true;

        PhotonNetwork.LocalPlayer.SetCustomProperties(RoomPlayerProperties);
    }

    public void ShowRoom()
    {
        gameObject.SetActive(true);

        RoomName.text = PhotonNetwork.CurrentRoom.Name;
        ShowSlot();
        ShowBtn();

        PhotonNetwork.LocalPlayer.SetCustomProperties(RoomPlayerProperties);
    }

    void ShowSlot()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                if (i < PhotonNetwork.PlayerList.Length)
                {
                    PSlots[i].AvailableOn();
                    PSlots[i].PlayerOn(PhotonNetwork.PlayerList[i].NickName);

                    if (PhotonNetwork.PlayerList[i].IsMasterClient)
                        PSlots[i].Owner.SetActive(true);
                }
                else
                {
                    PSlots[i].AvailableOn();
                    PSlots[i].PlayerOff();
                }
            }
            else
                PSlots[i].AvailableOff();
        }
    }

    void ShowBtn()
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == PhotonNetwork.MasterClient.ActorNumber)
        {
            ReadyBtn.SetActive(false);
            StartBtn.SetActive(true);
            StartBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            ReadyBtn.SetActive(true);
            StartBtn.SetActive(false);
        }
    }

    public void OnClickReadyBtn()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.GetValueOrDefault("IsReady").Equals(false))
        {
            RoomPlayerProperties["IsReady"] = true;
            PhotonNetwork.LocalPlayer.SetCustomProperties(RoomPlayerProperties);
        }
        else
        {
            RoomPlayerProperties["IsReady"] = false;
            PhotonNetwork.LocalPlayer.SetCustomProperties(RoomPlayerProperties);
        }
    }

    public void OnClickStartBtn()
    {
        RoomPlayerProperties["IsStart"] = true;

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            PhotonNetwork.PlayerList[i].SetCustomProperties(RoomPlayerProperties);
    }

    public void OnClickExitBtn()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == PhotonNetwork.MasterClient.ActorNumber)
            AssertionExit.SetActive(true);
        else
            ExitRoom();
    }

    public void OnClickExitBtnTwice()
    {
        ExitRoom();
    }

    void ExitRoom()
    {
        for (int i = 0; i < PlayerSlots.childCount; i++)
            Destroy(PlayerSlots.GetChild(i).gameObject);

        gameObject.SetActive(false);

        PhotonNetwork.LocalPlayer.SetCustomProperties(null);

        PhotonNetwork.LeaveRoom();
    }
}
