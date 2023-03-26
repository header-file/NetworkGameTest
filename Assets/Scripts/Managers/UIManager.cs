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
        Lobby.gameObject.SetActive(false);
        Room.gameObject.SetActive(false);
    }

    public void OpenLobby()
    {
        Lobby.gameObject.SetActive(true);
    }
}
