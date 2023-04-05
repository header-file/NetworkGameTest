using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UpperUI UpperUI;
    public Lobby Lobby;
    public RoomUI Room;
    public InGameUI InGameUI;

    public void CloseLobbyAndRoom()
    {
        InGameUI.gameObject.SetActive(true);
        Invoke("ShowPlayerSlot", 3.0f);

        Lobby.gameObject.SetActive(false);
        Room.gameObject.SetActive(false);
    }

    public void OpenLobby()
    {
        InGameUI.gameObject.SetActive(false);

        Lobby.gameObject.SetActive(true);
    }

    void ShowPlayerSlot()
    {
        InGameUI.ScoreUI.InsertSlots();

        GameManager.Inst().TurnManager.IsStartGame = true;
    }
}
