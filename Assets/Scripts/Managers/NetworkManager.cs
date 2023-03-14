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
        //RoomOptions options = new RoomOptions();
        //options.MaxPlayers = 4;
        //PhotonNetwork.JoinOrCreateRoom("Room1", options, null);
        //PhotonNetwork.JoinRoom("Room1");

        
    }

    public void RefreshRooms()
    {
        List<RoomInfo> roomList = new List<RoomInfo>();
        OnRoomListUpdate(roomList);

        for(int i = 0; i < roomList.Count; i++)
        {
            GameObject obj = Instantiate(RoomUIPref);
            Room room = obj.GetComponent<Room>();
            room.transform.parent = GameManager.Inst().UiManager.Lobby.transform;

            room.RoomName.text = roomList[i].Name;
            room.RoomMembers.text = roomList[i].PlayerCount + " / " + roomList[i].MaxPlayers;
        }
    }

    public void CreateRoom(string RoomName, int MaxPlayers)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)MaxPlayers;
        PhotonNetwork.CreateRoom(RoomName, options, null);

        Debug.Log("Create Room : " + RoomName);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        //PhotonNetwork.Instantiate("Player", SpawnPos, Quaternion.identity);
        Debug.Log("Joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + " : " + message);
    }

    void CreateRoom()
    {
        PhotonNetwork.CreateRoom("DefaultRoom", new RoomOptions { MaxPlayers = 4 });
    }
}
