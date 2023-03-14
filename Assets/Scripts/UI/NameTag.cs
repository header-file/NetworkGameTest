using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameTag : MonoBehaviour
{
    Transform MainCameraTransform;

    void Start()
    {
        MainCameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + MainCameraTransform.rotation * Vector3.forward,
            MainCameraTransform.rotation * Vector3.up);
    }
}
