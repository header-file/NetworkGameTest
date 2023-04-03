using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Vector3 SpawnPos;
    public GameObject RoomUIPref;


    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master Server");

        PhotonNetwork.JoinLobby();        
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Default Lobby");
    }

    public void RefreshRooms()
    {
        //PhotonNetwork.LocalPlayer.CustomProperties.Add(GAME_MODE, true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < GameManager.Inst().UiManager.Lobby.RoomSpace.childCount; i++)
            Destroy(GameManager.Inst().UiManager.Lobby.RoomSpace.GetChild(i).gameObject);

        for (int i = 0; i < roomList.Count; i++)
        {
            GameObject obj = Instantiate(RoomUIPref);
            Room room = obj.GetComponent<Room>();
            room.transform.SetParent(GameManager.Inst().UiManager.Lobby.RoomSpace);

            room.RoomName.text = roomList[i].Name;
            room.RoomMembers.text = roomList[i].PlayerCount + " / " + roomList[i].MaxPlayers;
        }
    }

    public void CreateRoom(string RoomName, int MaxPlayers)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)MaxPlayers;
        options.PublishUserId = true;
        //options.CustomRoomProperties = new Hashtable { { GAME_MODE, 1 } };
        PhotonNetwork.CreateRoom(RoomName, options, null);

        Debug.Log("Create Room : " + RoomName);
    }

    public override void OnCreatedRoom()
    {
        GameManager.Inst().UiManager.Room.CreateRoom();
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        GameManager.Inst().UiManager.Room.ShowRoom();

        Debug.Log("Joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + " : " + message);
    }

    //void CreateRoom()
    //{
    //    PhotonNetwork.CreateRoom("DefaultRoom", new RoomOptions { MaxPlayers = 4 });
    //}
}
