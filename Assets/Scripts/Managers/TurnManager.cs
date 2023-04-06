using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    /*
     Phase 0 : Standby Roll
     Phase 1 : Throw Dice
     Phase 2 : Dice Check
     */

    public Text Turn;

    public Box DiceBox;
    public bool IsStartGame;
    public int Phase;

    int TurnIndex;


    public int GetTurn() { return TurnIndex; }
    public void SetTurn(int turn) { TurnIndex = turn; }

    void Awake()
    {
        TurnIndex = 0;
        Phase = 0;
        IsStartGame = false;
    }

    void Update()
    {
        if (!IsStartGame)
            return;

        PhaseCheck();
        ShowTurnName();

        if (TurnIndex >= (12 * GameManager.Inst().PlayerCount))
            EndGame();

        ShowTurn();
    }

    void ShowTurnName()
    {
        if (CheckIsTurn())
            GameManager.Inst().UiManager.InGameUI.TurnText.text = GameManager.Inst().Player.PV.Owner.NickName + "'s Turn";
        else
            for (int i = 0; i < GameManager.Inst().OtherPlayers.Count; i++)
                if (GameManager.Inst().OtherPlayers[i].Index == (TurnIndex % GameManager.Inst().PlayerCount))
                    GameManager.Inst().UiManager.InGameUI.TurnText.text = GameManager.Inst().OtherPlayers[i].PV.Owner.NickName + "'s Turn";
    }

    void PhaseCheck()
    {
        if (Phase == 0)
        {
            GameManager.Inst().UiManager.InGameUI.DiceUI.gameObject.SetActive(false);
            DiceBox.Reroll();

            if (CheckIsTurn())
            {
                GameManager.Inst().UiManager.InGameUI.RollBtn.gameObject.SetActive(true);
                DiceBox.ShowDice(true);
            }
            else
            {
                GameManager.Inst().UiManager.InGameUI.RollBtn.gameObject.SetActive(false);
                DiceBox.ShowDice(false);
            }
        }
        else if (Phase == 1)
            GameManager.Inst().UiManager.InGameUI.RollBtn.gameObject.SetActive(false);
        else if (Phase == 2)
            GameManager.Inst().UiManager.InGameUI.DiceUI.gameObject.SetActive(true);
    }

    public bool CheckIsTurn()
    {
        if (!IsStartGame ||
            GameManager.Inst().Player.Index != (TurnIndex % GameManager.Inst().PlayerCount))
            return false;

        return true;
    }

    void EndGame()
    {
        GameManager.Inst().UiManager.InGameUI.WinnerName.text = GameManager.Inst().UiManager.InGameUI.ScoreUI.FindWinner();
        GameManager.Inst().UiManager.InGameUI.Winner.SetActive(true);
    }

    public void NextTurn()
    {
        TurnIndex++;

        Phase = 0;
    }

    void ShowTurn()
    {
        Turn.text = TurnIndex.ToString();
    }
}
