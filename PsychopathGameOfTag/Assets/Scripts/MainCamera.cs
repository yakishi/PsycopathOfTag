using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class MainCamera : MonoBehaviour
{
    GameObject targetObj;
    Vector3 targetPos;

    void Start()
    {
    }

    public void SetPlayer(GameObject obj)
    {
        targetObj = obj;
    }

    void Update()
    {
        if (targetObj == null) return;

        // targetの移動量分、自分（カメラ）も移動する
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;

        float rotateInputX = MyInput.Rotation().x;
        
        float mouseInputX = Input.GetAxis("Mouse X");
        
        // targetの位置のY軸を中心に、回転（公転）する
        transform.RotateAround(targetPos, Vector3.up, rotateInputX * Time.deltaTime * 200f);
        transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * 200f);

        // カメラの垂直移動（※角度制限なし、必要が無ければコメントアウト） 現状使用しないためコメント
        //transform.RotateAround(targetPos, transform.right, rotateInputY * Time.deltaTime * 200f);
    }
}