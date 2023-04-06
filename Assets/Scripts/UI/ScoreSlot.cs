using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScoreSlot : MonoBehaviour
{
    public ScorePlayerSlot Slot;
    public Text ScoreText;

    EventTrigger EvTrigger;
    bool IsScored;


    void Awake()
    {
        EvTrigger = GetComponent<EventTrigger>();
    }

    void Start()
    {
        IsScored = false;
    }

    public void ShowSample(int index)
    {
        if (IsScored || !Slot.IsWritable)
            return;

        int score = GameManager.Inst().ScoManager.Scores[index];

        ScoreText.color = Color.gray;
        ScoreText.text = score.ToString();
    }

    public void EraseSample()
    {
        if (IsScored)
            return;

        ScoreText.text = "";
    }

    public void ShowScore(int index)
    {
        if (IsScored || !Slot.IsWritable)
            return;

        int score = GameManager.Inst().ScoManager.Scores[index];

        ScoreText.color = Color.white;
        ScoreText.text = score.ToString();

        IsScored = true;
        Slot.IsWritable = false;

        GameManager.Inst().Player.SaveScore(index, score);
        GameManager.Inst().UiManager.InGameUI.ScoreUI.OnclickToggle();

        GameManager.Inst().TurnManager.NextTurn();
    }

    public void SetEventTrigger(bool IsEnable)
    {
        EvTrigger.enabled = IsEnable;
    }
}
