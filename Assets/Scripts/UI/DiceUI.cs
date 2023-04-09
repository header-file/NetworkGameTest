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


    public int GetReroll() { return Reroll; }
    public void SetRerollLeft(int count) { RerollLeft.text = "Left : " + count.ToString(); }

    void Start()
    {
        Reroll = 2;

        for(int i = 0; i < Locks.Length; i++)
            Locks[i].transform.GetChild(0).gameObject.SetActive(false);

        LockCount = 0;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if(GameManager.Inst().TurnManager.CheckIsTurn())
        {
            ConfirmBtn.interactable = true;
            RerollBtn.interactable = true;
        }
        else
        {
            ConfirmBtn.interactable = false;
            RerollBtn.interactable = false;
        }
    }

    public void ResetData()
    {
        Reroll = 2;
        LockCount = 0;

        for (int i = 0; i < Locks.Length; i++)
        {
            Locks[i].transform.GetChild(0).gameObject.SetActive(false);
            GameManager.Inst().TurnManager.DiceBox.Dices[i].IsLocked = false;
        }

        RerollLeft.text = "Left : " + Reroll.ToString();

        RerollBtn.interactable = true;
        ConfirmBtn.interactable = true;
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
        Reroll--;
        RerollLeft.text = "Left : " + Reroll.ToString();
        GameManager.Inst().TurnManager.Phase = 0;
        GameManager.Inst().TurnManager.DiceBox.Reroll();
    }

    public void OnClickConfirm()
    {
        GameManager.Inst().ScoManager.Calculate(GameManager.Inst().TurnManager.DiceBox.Nums);       
        GameManager.Inst().UiManager.InGameUI.ScoreUI.OnclickToggle();
        GameManager.Inst().UiManager.InGameUI.ScoreUI.LocalWrite();
    }

    public void OnClickRollBtn()
    {
        GameManager.Inst().TurnManager.DiceBox.Roll();
    }

    void OnEnable()
    {
        if (Reroll == 0 || Reroll == 2)
        {
            LockCount = 0;

            for (int i = 0; i < Locks.Length; i++)
            {
                Locks[i].transform.GetChild(0).gameObject.SetActive(false);
                GameManager.Inst().TurnManager.DiceBox.Dices[i].IsLocked = false;
            }

            RerollLeft.text = "Left : " + Reroll.ToString();

            RerollBtn.interactable = true;
            ConfirmBtn.interactable = true;
        }
    }
}
