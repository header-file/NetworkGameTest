using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonTest : MonoBehaviourPunCallbacks
{
    public Vector3 SpawnPos;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connect");
    }

    void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        //RoomOptions options = new RoomOptions();
        //options.MaxPlayers = 5;
        //PhotonNetwork.JoinOrCreateRoom("Room1", options, null);
    }

    public override void OnJoinedRoom()
    {
        GameObject p = PhotonNetwork.Instantiate("Player", SpawnPos, Quaternion.identity);
        //GameManager.Inst().Player = p.GetComponent<Player>();
        Debug.Log(p.transform.position);
    }
}
