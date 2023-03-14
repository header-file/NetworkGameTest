using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class Pad : MonoBehaviourPunCallbacks, IPointerDownHandler, IPointerUpHandler
{
    public Player Player;

    GameObject PadImg;

    bool IsPointerDown = false;
    Vector3 CenterPos;
    float PadRad;


    void Start()
    {
        PadImg = transform.GetChild(0).gameObject;
        PadImg.SetActive(false);
        IsPointerDown = false;
        PadRad = PadImg.GetComponent<RectTransform>().rect.width * 0.5f;
    }

    void Update()
    {
        if (!IsPointerDown)
            return;

        Move();
    }

    void Move()
    {
        Vector3 mPos = Input.mousePosition;

        Vector3 pos = mPos - CenterPos;
        Vector3 vec = Vector3.ClampMagnitude(CenterPos - pos, PadRad);
        pos = Vector3.ClampMagnitude(pos, PadRad);
        
        float sqr = vec.sqrMagnitude / (PadRad * PadRad);
        Vector2 normal = pos.normalized;

        Player.Move(normal, sqr);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        PadImg.SetActive(true);
        CenterPos = PadImg.transform.position = Input.mousePosition;

        IsPointerDown = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        PadImg.SetActive(false);

        IsPointerDown = false;

        Player.Stop();
    }
}
