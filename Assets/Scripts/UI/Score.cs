using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    //public ScorePlayerSlot ScorePlayerSlotPref;
    //public Transform SlotArea;
    public ScorePlayerSlot[] PlayerSlots;

    RectTransform RTransform;
    bool IsMoving;
    Vector2 GoalPos;


    void Awake()
    {
        RTransform = gameObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (IsMoving)
            Moving();
    }

    void Moving()
    {
        Vector2 pos = RTransform.anchoredPosition;
        RTransform.anchoredPosition = Vector2.MoveTowards(pos, GoalPos, Time.deltaTime * 2000.0f);

        if (Mathf.Abs(RTransform.anchoredPosition.x - GoalPos.x) <= 0.001f)
            IsMoving = false;
    }

    public void InsertSlots()
    {
        PlayerSlots[0].PlayerName.text = GameManager.Inst().Player.PV.Owner.NickName;
        PlayerSlots[0].UID = GameManager.Inst().Player.PV.Owner.UserId;PlayerSlots[0].SetSlotTrigger(true);
        PlayerSlots[0].Self = GameManager.Inst().Player;
        GameManager.Inst().Player.SetIndex();

        for (int i = 1; i <= GameManager.Inst().OtherPlayers.Count; i++)
        {
            PlayerSlots[i].PlayerName.text = GameManager.Inst().OtherPlayers[i - 1].PV.Owner.NickName;
            PlayerSlots[i].UID = GameManager.Inst().OtherPlayers[i - 1].PV.Owner.UserId;
            PlayerSlots[i].SetSlotTrigger(false);
            PlayerSlots[i].Self = GameManager.Inst().OtherPlayers[i - 1];
            GameManager.Inst().OtherPlayers[i - 1].SetIndex();
        }

        if(GameManager.Inst().PlayerCount < 4)
            for (int i = GameManager.Inst().PlayerCount; i < 4; i++)
                PlayerSlots[i].gameObject.SetActive(false);
    }

    public void LocalWrite()
    {
        PlayerSlots[0].IsWritable = true;
    }

    public void GetScoreData(string uid, int index, string val)
    {
        if (PlayerSlots == null)
            return;

        for(int i = 0; i < PlayerSlots.Length; i++)
        {
            if(PlayerSlots[i].UID == uid)
                PlayerSlots[i].ScoreSlots[index].ScoreText.text = val;
        }
    }

    public void OnclickToggle()
    {
        if (IsMoving || PlayerSlots[0].IsWritable)
            return;

        IsMoving = true;

        if (RTransform.anchoredPosition.x > -1000.0f)
            GoalPos = new Vector2(-1400.0f, 0.0f);
        else
            GoalPos = Vector2.zero;
    }

    public string FindWinner()
    {
        int max = 0;
        string name = "";
        for(int i = 0; i < PlayerSlots.Length; i++)
        {
            int val = int.Parse(PlayerSlots[i].Total.text);
            if(val > max)
            {
                name = PlayerSlots[i].PlayerName.text;
                max = val;
            }
        }

        return name;
    }
}
