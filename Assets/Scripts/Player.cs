using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView PV;
    public Hashtable ScoreTable;
    public int Index;


    void Awake()
    {
        ScoreTable = new Hashtable();
        for (int i = 0; i < 12; i++)
            ScoreTable.Add(GameManager.Inst().GetScoreName(i), 0);

        if (PV.IsMine)
        {
            GameManager.Inst().Player = gameObject.GetComponent<Player>();
            Index = int.Parse(PV.Owner.CustomProperties["Index"].ToString());
        }
        else
            GameManager.Inst().OtherPlayers.Add(gameObject.GetComponent<Player>());
    }

    void Update()
    {
        if (!PV.IsMine || !PhotonNetwork.IsConnected)
            return;
    }

    public void SaveScore(int index, int score)
    {
        ScoreTable[GameManager.Inst().GetScoreName(index)] = score;        
    }

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(ScoreTable);
            stream.SendNext(Index);
        }
        else
        {
            ScoreTable = (Hashtable)stream.ReceiveNext();
            Index = (int)stream.ReceiveNext();

            for (int i = 0; i < 12; i++)
                GameManager.Inst().UiManager.InGameUI.ScoreUI.GetScoreData(PV.Owner.UserId, i, ScoreTable[GameManager.Inst().GetScoreName(i)].ToString());
        }
    }

    #endregion
}
