using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public Text RoomName;
    public Text RoomMembers;


    public void TryJoinRoom()
    {
        GameManager.Inst().NetManager.JoinRoom(RoomName.text);
    }
}
