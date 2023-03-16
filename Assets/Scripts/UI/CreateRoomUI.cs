using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomUI : MonoBehaviour
{
    public InputField RoomName;
    public Text MaxPlayerLabel;


    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void OnClickCreateBtn()
    {
        string name = RoomName.text;
        int max = int.Parse(MaxPlayerLabel.text);

        GameManager.Inst().NetManager.CreateRoom(name, max);

        gameObject.SetActive(false);
    }
}
