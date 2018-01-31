using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NewSyncPosition : NetworkBehaviour {

    // 歩行速度（メートル/秒）
    [SerializeField] float m_WalkSpeed = 4f;

    // 旋回速度（度/秒）
    [SerializeField] float m_RotateSpeed = 180f;

    // Lerpの係数
    [SerializeField] float m_LerpRate = 4f;

    // ホストから受信した位置情報
    Vector3 m_ReceivedPosition;

    // ホストから受信した回転情報
    Quaternion m_ReceivedRotation;

    void Start()
    {
        m_ReceivedPosition = transform.position;
        m_ReceivedRotation = transform.rotation;
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            // 旋回
            transform.Rotate(0, Input.GetAxis("Horizontal") * m_RotateSpeed * Time.deltaTime, 0);

            // 移動
            transform.Translate(0, 0, Input.GetAxis("Vertical") * m_WalkSpeed * Time.deltaTime, Space.Self);

            // 位置情報をホストへ送信
            CmdSyncTransform(transform.position, transform.rotation);
        }
        else
        {
            // 補間して移動
            InterpolateTransform();
        }

    }

    // ホストから受信した位置・回転に、補間しながら近づける
    void InterpolateTransform()
    {
        Vector3 pos = Vector3.Lerp(transform.position, m_ReceivedPosition, m_LerpRate * Time.deltaTime);
        Quaternion rot = Quaternion.Slerp(transform.rotation, m_ReceivedRotation, m_LerpRate * Time.deltaTime);
        transform.SetPositionAndRotation(pos, rot);
    }

    // 位置情報をホストに送信するCommand
    [Command]
    void CmdSyncTransform(Vector3 position, Quaternion rotation)
    {
        // 各接続に対して情報を送信する
        foreach (var conn in NetworkServer.connections)
        {
            // 無効なConnectionは無視する
            if (conn == null || !conn.isReady)
                continue;

            // 自分（情報発信元）に送り返してもしょうがない（というかむしろ有害）なので、
            // 自分へのConnectionは無視する
            if (conn == connectionToClient)
                continue;

            // このConnectionに対して位置情報を送信する
            TargetSyncTransform(conn, position, rotation);
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
