using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public ScorePlayerSlot ScorePlayerSlotPref;
    public Transform SlotArea;

    ScorePlayerSlot[] PlayerSlots;
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
        PlayerSlots = new ScorePlayerSlot[GameManager.Inst().OtherPlayers.Count + 1];

        PlayerSlots[0] = Instantiate(ScorePlayerSlotPref);
        PlayerSlots[0].PlayerName.text = GameManager.Inst().Player.PV.Owner.NickName;
        PlayerSlots[0].UID = GameManager.Inst().Player.PV.Owner.UserId;
        PlayerSlots[0].transform.SetParent(SlotArea);

        for (int i = 1; i <= GameManager.Inst().OtherPlayers.Count; i++)
        {
            PlayerSlots[i] = Instantiate(ScorePlayerSlotPref);
            PlayerSlots[i].PlayerName.text = GameManager.Inst().OtherPlayers[i - 1].PV.Owner.NickName;
            PlayerSlots[i].UID = GameManager.Inst().OtherPlayers[i - 1].PV.Owner.UserId;
            PlayerSlots[i].transform.SetParent(SlotArea);
        }
    }

    public void LocalWrite()
    {
        PlayerSlots[0].IsWritable = true;
    }

    public void OnclickToggle()
    {
        if (IsMoving)
            return;

        IsMoving = true;

        if (RTransform.anchoredPosition.x > -1000.0f)
            GoalPos = new Vector2(-1400.0f, 0.0f);
        else
            GoalPos = Vector2.zero;

        if(PlayerSlots.Length != GameManager.Inst().OtherPlayers.Count + 1)
        {
            for (int i = 0; i < SlotArea.childCount; i++)
                Destroy(SlotArea.GetChild(i).gameObject);

            InsertSlots();
        }
    }
}
