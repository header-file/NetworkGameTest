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

    public GameObject PlayerSlotPref;
    public GameObject EmptySlotPref;

    Hashtable RoomPlayerProperties;
    PlayerSlot[] PSlots;


    void Awake()
    {
        RoomPlayerProperties = new Hashtable();
        RoomPlayerProperties.Add("IsReady", false);

        AssertionExit.SetActive(false);
        gameObject.SetActive(false);
    }

    void Update()
    {
        PlayerUpdate();
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

                        if (PhotonNetwork.PlayerList[i].CustomProperties != null &&
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

    public void CreateRoom()
    {
        RoomPlayerProperties.Clear();
        RoomPlayerProperties.Add("IsReady", true);

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
        PSlots = new PlayerSlot[4];

        for (int i = 0; i < 4; i++)
        {
            if (i < PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                if (i < PhotonNetwork.PlayerList.Length)
                {
                    GameObject slot = Instantiate(PlayerSlotPref);
                    slot.transform.SetParent(PlayerSlots);
                    PlayerSlot pSlot = slot.GetComponent<PlayerSlot>();
                    pSlot.PlayerOn(PhotonNetwork.PlayerList[i].NickName);

                    if (PhotonNetwork.PlayerList[i].IsMasterClient)
                        pSlot.Owner.SetActive(true);
                }
                else
                {
                    GameObject slot = Instantiate(PlayerSlotPref);
                    slot.transform.SetParent(PlayerSlots);
                }
            }
            else
            {
                GameObject slot = Instantiate(EmptySlotPref);
                slot.transform.SetParent(PlayerSlots);
            }
        }

        for (int i = 0; i < 4; i++)
            if (PlayerSlots.GetChild(i) != null)
                PSlots[i] = PlayerSlots.GetChild(i).GetComponent<PlayerSlot>();
    }

    void ShowBtn()
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == PhotonNetwork.MasterClient.ActorNumber)
        {
            ReadyBtn.SetActive(false);
            StartBtn.SetActive(true);
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
            RoomPlayerProperties.Clear();
            RoomPlayerProperties.Add("IsReady", true);
            PhotonNetwork.LocalPlayer.SetCustomProperties(RoomPlayerProperties);
        }
        else
        {
            RoomPlayerProperties.Clear();
            RoomPlayerProperties.Add("IsReady", false);
            PhotonNetwork.LocalPlayer.SetCustomProperties(RoomPlayerProperties);
        }
    }

    public void OnClickStartBtn()
    {
        
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
