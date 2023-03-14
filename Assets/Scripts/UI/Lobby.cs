using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    public CreateRoomUI CreateRoomUI;
    public Transform RoomSpace;


    public void OnClickCreateBtn()
    {
        CreateRoomUI.gameObject.SetActive(true);
    }

    public void OnClickRefreshBtn()
    {
        GameManager.Inst().NetManager.RefreshRooms();   
    }
}
