using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

public class ScorePlayerSlot : MonoBehaviour
{
    public Text PlayerName;
    public ScoreSlot[] ScoreSlots;
    public Text SubTotal;
    public Text Total;

    public Player Self;

    public string UID;
    public bool IsWritable;


    void Awake()
    {
        IsWritable = false;
    }

   void Update()
    {
        CalculateTotal();
    }

    public void CalculateTotal()
    {
        int subTotal = 0;
        for (int i = 0; i < 6; i++)
            subTotal += ScoreSlots[i].ScoreText.text == "" ? 0 : int.Parse(ScoreSlots[i].ScoreText.text);
        SubTotal.text = subTotal.ToString();

        int total = subTotal;
        for (int i = 6; i < 12; i++)
            total += ScoreSlots[i].ScoreText.text == "" ? 0 : int.Parse(ScoreSlots[i].ScoreText.text);
        Total.text = total.ToString();
    }

    public void SetSlotTrigger(bool IsEnable)
    {
        for (int i = 0; i < ScoreSlots.Length; i++)
            ScoreSlots[i].SetEventTrigger(IsEnable);
    }
}
