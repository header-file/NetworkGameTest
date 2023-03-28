using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePlayerSlot : MonoBehaviour
{
    public Text PlayerName;

    public ScoreSlot Ones;
    public ScoreSlot Twos;
    public ScoreSlot Threes;
    public ScoreSlot Fours;
    public ScoreSlot Fives;
    public ScoreSlot Sixes;
    public Text SubTotal;

    public ScoreSlot FoaK;
    public ScoreSlot FullHouse;
    public ScoreSlot LStraight;
    public ScoreSlot BStraight;
    public ScoreSlot Yacht;
    public ScoreSlot Choice;
    public Text Total;

    
    void Awake()
    {

    }

    void Updat()
    {
        CalculateTotal();
    }

    public void CalculateTotal()
    {
        int subTotal = 0;
        subTotal += Ones.ScoreText.text == "" ? 0 : int.Parse(Ones.ScoreText.text);
        subTotal += Twos.ScoreText.text == "" ? 0 : int.Parse(Twos.ScoreText.text);
        subTotal += Threes.ScoreText.text == "" ? 0 : int.Parse(Threes.ScoreText.text);
        subTotal += Fours.ScoreText.text == "" ? 0 : int.Parse(Fours.ScoreText.text);
        subTotal += Fives.ScoreText.text == "" ? 0 : int.Parse(Fives.ScoreText.text);
        subTotal += Sixes.ScoreText.text == "" ? 0 : int.Parse(Sixes.ScoreText.text);
        SubTotal.text = subTotal.ToString();

        int total = subTotal;
        total += FoaK.ScoreText.text == "" ? 0 : int.Parse(FoaK.ScoreText.text);
        total += FullHouse.ScoreText.text == "" ? 0 : int.Parse(FullHouse.ScoreText.text);
        total += LStraight.ScoreText.text == "" ? 0 : int.Parse(LStraight.ScoreText.text);
        total += BStraight.ScoreText.text == "" ? 0 : int.Parse(BStraight.ScoreText.text);
        total += Yacht.ScoreText.text == "" ? 0 : int.Parse(Yacht.ScoreText.text);
        total += Choice.ScoreText.text == "" ? 0 : int.Parse(Choice.ScoreText.text);
        Total.text = total.ToString();
    }
}
