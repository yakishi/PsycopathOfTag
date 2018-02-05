using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyInput:NetworkBehaviour{

    public static bool isMove()
    {
        return !(HorizontalAxis() == 0 && VerticallAxis() == 0);
    }
    public static bool isRotate()
    {
        return !(RotateHorizontalAxis() == 0);
    }

    public static float HorizontalAxis()
    {
        return Input.GetAxis("Horizontal");
    }
    public static float VerticallAxis()
    {
        return Input.GetAxis("Vertical");
    }

    public static float RotateHorizontalAxis()
    {
        return Input.GetAxis("RotateHorizontal");
    }

    public static Vector3 Direction()
    {
        Vector3 ret = Vector3.zero;
        if (Input.GetAxis("Horizontal") >= 1.0f || Input.GetKey(KeyCode.D)) {
            ret += Vector3.right;
        }
        else if (Input.GetAxis("Horizontal") <= -1.0f || Input.GetKey(KeyCode.A)) {
            ret += Vector3.left;
        }

        if (Input.GetAxis("Vertical") >= 1.0f || Input.GetKey(KeyCode.S)) {
            ret += Vector3.back;
        }
        else if (Input.GetAxis("Vertical") <= -1.0f || Input.GetKey(KeyCode.W)) {
            ret += Vector3.forward;
        }

        ret.Normalize();
        return ret;
    }

    public static Vector3 Rotation()
    {
        Vector3 ret = Vector3.zero;

        ret = new Vector3(RotateHorizontalAxis(),0,0);

        return ret;
    }
}
