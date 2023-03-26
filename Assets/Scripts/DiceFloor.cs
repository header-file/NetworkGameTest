using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceFloor : MonoBehaviour
{
    Dice Dice;
    float Count;


    void Awake()
    {
        Dice = transform.parent.GetComponent<Dice>();
    }

    //void OnTriggerStay(Collider other)
    //{
    //    if (other.tag != "Floor" || !Dice.IsRolling)
    //        return;

    //    Count += Time.deltaTime;

    //    if (Count >= 1.0f)
    //    {
    //        Dice.Number = int.Parse(gameObject.name);
    //        Dice.IsRolling = false;
    //        Debug.Log(Dice.Number);
    //    }
    //}

    void OnTriggerExit(Collider other)
    {
        if (other.tag != "Floor")
            return;

        Count = 0;
    }
}
