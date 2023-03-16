using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Nickname : MonoBehaviour
{
    public InputField NameInput;


    public void OnClickConfirmBtn()
    {
        PhotonNetwork.LocalPlayer.NickName = NameInput.text;

        gameObject.SetActive(false);
    }
}
