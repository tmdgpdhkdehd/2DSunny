using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject player;  //A라는 GameObject변수 선언
    Transform AT;
    void Start()
    {
        AT = player.transform;
    }
    void LateUpdate()
    {
        transform.position = new Vector3(AT.position.x, AT.position.y, -10);
    }
}
