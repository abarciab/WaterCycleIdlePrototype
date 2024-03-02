using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    void Update()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 0;
        transform.position = mousePos;
    }
}
