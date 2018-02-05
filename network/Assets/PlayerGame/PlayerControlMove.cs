using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControlMove : NetworkBehaviour {

    // Lerpの係数
    [SerializeField] float m_LerpRate = 4f;

    // ホストから受信した位置情報
    Vector3 m_ReceivedPosition;

    // ホストから受信した回転情報
    Quaternion m_ReceivedRotation;

    private float inputHorizontal;
    private float inputVertical;
    private Rigidbody rb;

    private float moveSpeed = 0.5f;

    public GameObject bulletprehub;
    GameObject player;
    GameObject camera;

    public void Start()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        m_ReceivedRotation = transform.rotation;
        m_ReceivedPosition = transform.position;
        player = GameObject.Find("Player");
        camera = GameObject.Find("Camera");
    }
    public override void OnStartLocalPlayer()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!hasAuthority)     //これがないと自身の操作を分別できない
        {
            LerpPosition();
        }
        else
        {
            inputHorizontal = MyInput.Direction().x;
            inputVertical = MyInput.Direction().z;

            // カメラの方向から、X-Z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

            // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
            rb.velocity = moveForward * moveSpeed + new Vector3(0, rb.velocity.y, 0);

            // キャラクターの向きを進行方向に
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }

            CmdMove(rb.velocity, transform.rotation);
        }
    }

    private void LerpPosition() //補間
    {
        Vector3 pos = Vector3.Lerp(transform.position, m_ReceivedPosition, m_LerpRate * Time.deltaTime);
        Quaternion rot = Quaternion.Slerp(transform.rotation, m_ReceivedRotation, m_LerpRate * Time.deltaTime);
        transform.SetPositionAndRotation(pos, rot);
    }

    [Command]
    void CmdMove(Vector3 vec3, Quaternion rot)
    {
        foreach (var conn in NetworkServer.connections)
        {
            // 無効なConnectionは無視する
            if (conn == null || !conn.isReady)
                continue;

            // 自分（情報発信元）に送り返してもしょうがない（というかむしろ有害）なので、
            // 自分へのConnectionは無視する
            if (conn == connectionToClient)
                continue;

            TargetSyncTransform(conn, vec3, rot);
        }
    }

    // クライアント側で位置情報を受け取り、オブジェクトに反映させる
    [TargetRpc]
    void TargetSyncTransform(NetworkConnection target, Vector3 position, Quaternion rotation)
    {
        m_ReceivedPosition = position;
        m_ReceivedRotation = rotation;
    }
}
