using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceUI : MonoBehaviour
{
    public GameObject[] Locks;
    public Button RerollBtn;
    public Text RerollLeft;
    public Button ConfirmBtn;

    int Reroll;
    int LockCount;


    void Start()
    {
        Reroll = 2;

        for(int i = 0; i < Locks.Length; i++)
            Locks[i].transform.GetChild(0).gameObject.SetActive(false);

        LockCount = 0;
        gameObject.SetActive(false);
    }

    public void OnClickLock(int index)
    {
        if (!GameManager.Inst().TurnManager.DiceBox.Dices[index].IsLocked)
        {
            Locks[index].transform.GetChild(0).gameObject.SetActive(true);
            GameManager.Inst().TurnManager.DiceBox.Dices[index].IsLocked = true;
            LockCount++;
        }
        else
        {
            Locks[index].transform.GetChild(0).gameObject.SetActive(false);
            GameManager.Inst().TurnManager.DiceBox.Dices[index].IsLocked = false;
            LockCount--;
        }

        if (LockCount >= Locks.Length)
            RerollBtn.interactable = false;
        else
            RerollBtn.interactable = true;
    }

    public void OnClickReroll()
    {
        GameManager.Inst().TurnManager.DiceBox.Reroll();
        RerollLeft.text = "Left : " + (--Reroll).ToString();

        gameObject.SetActive(false);
    }

    public void OnClickConfirm()
    {
        GameManager.Inst().ScoManager.Calculate(GameManager.Inst().TurnManager.DiceBox.Nums);       
        GameManager.Inst().UiManager.InGameUI.ScoreUI.OnclickToggle();
    }

    void OnEnable()
    {
        if (Reroll > 0)
            return;

        RerollBtn.interactable = false;
    }
}
