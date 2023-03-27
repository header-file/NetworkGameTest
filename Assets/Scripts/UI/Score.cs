using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
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
        RTransform.anchoredPosition = Vector2.Lerp(pos, GoalPos, Time.deltaTime * 2.0f);

        if (Mathf.Abs(RTransform.anchoredPosition.x - GoalPos.x) <= 0.001f)
            IsMoving = false;
    }

    public void OnclickToggle()
    {
        IsMoving = true;

        if (RTransform.anchoredPosition.x > -1000.0f)
            GoalPos = new Vector2(-1400.0f, 0.0f);
        else
            GoalPos = Vector2.zero;
    }
}
