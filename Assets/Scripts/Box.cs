using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Dice[] Dices;
    public int[] Nums;

    Quaternion[] Rots;
    bool IsRollable;
    bool IsStartCheck;
    int LockCount;


    void Start()
    {
        Nums = new int[5];
        Rots = new Quaternion[6];
        for (int i = 0; i < Nums.Length; i++)
        {
            Nums[i] = -1;

            Rots[i] = Dices[i].transform.rotation;
        }
        Rots[5] = Quaternion.Euler(90.0f, 0.0f, -180.0f);

        IsRollable = true;
        IsStartCheck = false;
        LockCount = 0;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && IsRollable)
            for (int i = 0; i < Dices.Length; i++)
                Dices[i].StartRoll();
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

        GameManager.Inst().UiManager.InGameUI.DiceUI.gameObject.SetActive(true);

        IsRollable = false;
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
        IsRollable = true;
    }
}
