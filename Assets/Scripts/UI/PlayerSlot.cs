using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerSlot : MonoBehaviour
{
    public GameObject Available;
    public GameObject NotAvailable;

    public GameObject Ready;
    public GameObject Owner;
    public GameObject Empty;
    public Text PlayerName;

    void Awake()
    {
        Ready.SetActive(false);
        Owner.SetActive(false);
        PlayerName.gameObject.SetActive(false);
        Empty.SetActive(true);
        PlayerName.text = "";
    }

    public void PlayerOn(string userId)
    {
        PlayerName.gameObject.SetActive(true);
        Empty.SetActive(false);
        PlayerName.text = userId;
    }

    public void PlayerOff()
    {
        Ready.SetActive(false);
        PlayerName.gameObject.SetActive(false);
        Empty.SetActive(true);
    }

    public void AvailableOn()
    {
        Available.SetActive(true);
        NotAvailable.SetActive(false);
    }

    public void AvailableOff()
    {
        Available.SetActive(false);
        NotAvailable.SetActive(true);
    }
}
