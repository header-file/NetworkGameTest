using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Box : MonoBehaviour
{
    public Dice[] Dices;
    public int[] Nums;
    public Vector3[] Euls;
    public Vector3[] Poses;

    Quaternion[] Rots;
    bool IsStartCheck;
    int LockCount;


    void Start()
    {
        Nums = new int[5];
        Rots = new Quaternion[6];
        for (int i = 0; i < Nums.Length; i++)
        {
            Nums[i] = -1;

            Rots[i] = Quaternion.Euler(Euls[i]);
        }

        IsStartCheck = false;
        LockCount = 0;
    }

    public void Roll()
    {
        if ((GameManager.Inst().Player.Index != GameManager.Inst().TurnManager.TurnIndex) ||
            GameManager.Inst().TurnManager.Phase != 0)
            return;

        for (int i = 0; i < Dices.Length; i++)
            Dices[i].StartRoll();

        GameManager.Inst().TurnManager.Phase = 1;
    }
    
    RaycastHit CastRay()
    {
        Vector3 screenMouseFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenMoseNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMouseFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMoseNear);

        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }

    public void StartCheck()
    {
        if (!GameManager.Inst().TurnManager.CheckIsTurn())
            return;

        if (!IsStartCheck)
        {
            IsStartCheck = true;
            CancelInvoke("DiceCheck");
            Invoke("DiceCheck", 2.0f);
        }
    }

    void DiceCheck()
    {
        for(int i = 0; i < Dices.Length; i++)
        {
            Nums[i] = Dices[i].Number;
            Dices[i].ResetPos(Rots[Nums[i] - 1]);
        }

        GameManager.Inst().TurnManager.Phase = 2;

        IsStartCheck = false;
    }

    public void Reroll()
    {
        for (int i = 0; i < Dices.Length; i++)
        {
            if (!Dices[i].IsLocked)
                Dices[i].IsRolling = false;
            else
                Dices[i].StayMode(new Vector3(-25.0f, 0.25f, -18.0f + 5.0f * LockCount++), Rots[Nums[i] - 1]);
        }

        LockCount = 0;
    }

    public void MakeDice()
    {
        Dices = new Dice[5];

        for (int i = 0; i < Dices.Length; i++)
        {
            GameObject dice = PhotonNetwork.Instantiate("Prefabs/Dice", Poses[i], Rots[i]);
            Dices[i] = dice.GetComponent<Dice>();
            Dices[i].SetOwner();
        }

        ShowDice(false);
    }

    public void SetOwner()
    {
        for (int i = 0; i < Dices.Length; i++)
            Dices[i].SetOwner();
    }

    public void ShowDice(bool IsShow)
    {
        for (int i = 0; i < Dices.Length; i++)
        {
            Dices[i].transform.position = (IsShow == true ? Poses[i] : new Vector3(0.0f, -50.0f, 0.0f));
            Dices[i].Show(IsShow);
        }
    }
}
