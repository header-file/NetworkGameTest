using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurnManager : MonoBehaviour, IPunObservable
{
    /*
     Phase 0 : Standby Roll
     Phase 1 : Throw Dice
     Phase 2 : Dice Check
     */

    public Box DiceBox;
    public bool IsStartGame;
    public int TurnIndex;
    public int Phase;


    void Awake()
    {
        TurnIndex = -1;
        IsStartGame = false;
    }

    void Update()
    {
        if (!IsStartGame)
            return;

        PhaseCheck();
        ShowTurnName();

        if (TurnIndex >= (12 * (GameManager.Inst().OtherPlayers.Count + 1)))
            EndGame();
    }

    void ShowTurnName()
    {
        if (CheckIsTurn())
            GameManager.Inst().UiManager.InGameUI.TurnText.text = GameManager.Inst().Player.PV.Owner.NickName + "'s Turn";
        else
            for (int i = 0; i < GameManager.Inst().OtherPlayers.Count; i++)
                if (GameManager.Inst().OtherPlayers[i].Index == (TurnIndex % (GameManager.Inst().OtherPlayers.Count + 1)))
                    GameManager.Inst().UiManager.InGameUI.TurnText.text = GameManager.Inst().OtherPlayers[i].PV.Owner.NickName + "'s Turn";
    }

    void PhaseCheck()
    {
        if (Phase == 0)
        {
            GameManager.Inst().UiManager.InGameUI.DiceUI.gameObject.SetActive(false);
            DiceBox.Reroll();

            if (CheckIsTurn())
                GameManager.Inst().UiManager.InGameUI.RollBtn.gameObject.SetActive(true);
            else
                GameManager.Inst().UiManager.InGameUI.RollBtn.gameObject.SetActive(false);
        }
        else if (Phase == 1)
            GameManager.Inst().UiManager.InGameUI.RollBtn.gameObject.SetActive(false);
        else if (Phase == 2)
            GameManager.Inst().UiManager.InGameUI.DiceUI.gameObject.SetActive(true);
    }

    public bool CheckIsTurn()
    {
        if (!IsStartGame)
            return false;

        if (GameManager.Inst().Player.Index == (TurnIndex % (GameManager.Inst().OtherPlayers.Count + 1)))
            return true;

        return false;
    }

    void EndGame()
    {

    }

    public void NextTurn()
    {
        TurnIndex++;

        Phase = 0;
    }

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(TurnIndex);
            stream.SendNext(Phase);
        }
        else
        {
            TurnIndex = (int)stream.ReceiveNext();
            Phase = (int)stream.ReceiveNext();
        }
    }

    #endregion
}
