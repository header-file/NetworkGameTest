using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UIManager : MonoBehaviour
{
    public UpperUI UpperUI;
    public Lobby Lobby;
    public RoomUI Room;
    public InGameUI InGameUI;
    public GameObject ChatUI;


    public void CloseLobbyAndRoom()
    {
        InGameUI.gameObject.SetActive(true);
        Invoke("ShowPlayerSlot", 3.0f);

        Lobby.gameObject.SetActive(false);
        Room.gameObject.SetActive(false);
    }

    public void OpenLobbyAndRoom()
    {
        InGameUI.gameObject.SetActive(false);

        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            PhotonNetwork.LocalPlayer.CustomProperties["IsReady"] = false;

        Lobby.gameObject.SetActive(true);
        Room.gameObject.SetActive(true);
    }

    void ShowPlayerSlot()
    {
        InGameUI.ScoreUI.InsertSlots();

        GameManager.Inst().TurnManager.IsStartGame = true;
    }
}
