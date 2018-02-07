using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour {

    Player player;


    // Lerpの係数
    [SerializeField] float m_LerpRate = 4f;
    // ホストから受信した位置情報
    Vector3 m_ReceivedPosition;

    // ホストから受信した回転情報
    Quaternion m_ReceivedRotation;

    //[SerializeField]
    //AnimationController animController;

    private float inputHorizontal;
    private float inputVertical;
    private Rigidbody rb;

    private float moveSpeed = 3.0f;

    // Use this for initialization
    void Start () {
        player = gameObject.GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!hasAuthority) {
            LerpPosition();
            return;
        }

        if(player == null) player = gameObject.GetComponent<Player>();

        if (player.isDead || player.catchTrap || !isLocalPlayer) return;

        inputHorizontal = MyInput.Direction().x;
        inputVertical = MyInput.Direction().z;

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

        CmdMove(this.transform.position);
    }

    private void LerpPosition() //補間
    {

        Vector3 pos = Vector3.Lerp(transform.position, m_ReceivedPosition, m_LerpRate * Time.deltaTime);

        transform.position = pos;
    }

    [Command]
    void CmdMove(Vector3 vec3)
    {
        foreach (var conn in NetworkServer.connections) {
            // 無効なConnectionは無視する
            if (conn == null || !conn.isReady)
                continue;

            // 自分（情報発信元）に送り返してもしょうがない（というかむしろ有害）なので、
            // 自分へのConnectionは無視する
            if (conn == connectionToClient)
                continue;

            TargetSyncTransform(conn, vec3);
        }
    }

    // クライアント側で位置情報を受け取り、オブジェクトに反映させる
    [TargetRpc]
    void TargetSyncTransform(NetworkConnection target, Vector3 position)
    {
        //transform.SetPositionAndRotation(position, rotation);
        //transform.position = position;
        m_ReceivedPosition = position;
    }
}
