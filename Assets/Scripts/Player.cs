using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView PV;
    public string[] ScoreTable;
    public int Index;


    void Awake()
    {
        ScoreTable = new string[12];
        for (int i = 0; i < 12; i++)
            ScoreTable[i] = "";

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
        ScoreTable[index] = score.ToString();        
    }

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            for (int i = 0; i < ScoreTable.Length; i++)
                stream.SendNext(ScoreTable[i]);

            stream.SendNext(Index);
        }
        else
        {
            for (int i = 0; i < ScoreTable.Length; i++)
            {
                this.ScoreTable[i] = (string)stream.ReceiveNext();

                GameManager.Inst().UiManager.InGameUI.ScoreUI.GetScoreData(PV.Owner.UserId, i, ScoreTable[i]);
            }

            this.Index = (int)stream.ReceiveNext();
        }
    }

    #endregion
}
