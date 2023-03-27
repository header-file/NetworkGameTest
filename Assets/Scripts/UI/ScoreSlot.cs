using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSlot : MonoBehaviour
{
    public Text ScoreText;

    bool IsScored;


    void Start()
    {
        IsScored = false;
    }

    public void ShowSample(int index)
    {
        if (IsScored)
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
        if (IsScored)
            return;

        int score = GameManager.Inst().ScoManager.Scores[index];

        ScoreText.color = Color.white;
        ScoreText.text = score.ToString();

        IsScored = true;
    }
}
