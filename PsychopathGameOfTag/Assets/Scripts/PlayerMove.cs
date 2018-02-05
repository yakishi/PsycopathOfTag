using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    Player player;

    //[SerializeField]
    //AnimationController animController;

    private float inputHorizontal;
    private float inputVertical;
    private Rigidbody rb;

    private float moveSpeed = 10.0f;

    // Use this for initialization
    void Start () {
        player = gameObject.GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {


        if(player == null) player = gameObject.GetComponent<Player>();

        if (player.isDead || player.catchTrap) return;

        inputHorizontal = MyInput.Direction().x;
        inputVertical = MyInput.Direction().z;

        /*if (MyInput.isMove()) {
            animController.PlayAnimation("Moving");
        }
        else {
            animController.StopAnimation("Moving");
        }*/

        // カメラの方向から、X-Z平面の単位ベクトルを取得
        player.cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        
        // 方向キーの入力値とカメラの向きから、移動方向を決定
        player.moveForward = player.cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;


        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        rb.velocity = player.moveForward * moveSpeed + new Vector3(0, rb.velocity.y, 0);

        // キャラクターの向きを進行方向に
        if (player.moveForward != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(player.moveForward);
        }
    }
}
