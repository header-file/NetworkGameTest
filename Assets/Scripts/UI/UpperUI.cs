using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpperUI : MonoBehaviour
{
    public Text BulletTxt;

    public void SetBulletTxt(string Str) { BulletTxt.text = Str; }
}
