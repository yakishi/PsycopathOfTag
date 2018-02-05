using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityStandardAssets.Utility;

public class PlayerMove : NetworkBehaviour {

    // Lerpの係数
    [SerializeField] float m_LerpRate = 4f;

    // ホストから受信した位置情報
    Vector3 m_ReceivedPosition;

    // ホストから受信した回転情報
    Quaternion m_ReceivedRotation;

    int muki = 6;
    public float movespeed;
    public GameObject bulletprehub;
    PlayerSpawn Spawn;
	// Use this for initialization
	void Start (){ 

    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        Spawn = this.GetComponent<PlayerSpawn>();
        Camera.main.GetComponent<FollowTarget>().target = transform;
        Camera.main.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update () {
        
        if (!hasAuthority)     //これがないと自身の操作を分別できない
        {
            LerpPosition();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CmdFire(muki);
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            {
                var x = Input.GetAxis("Horizontal") * movespeed;
                this.transform.Translate(x, 0, 0);
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    muki = 3;
                }else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    muki = 9;
                }
            }
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                var z = Input.GetAxis("Vertical") * movespeed;
                this.transform.Translate(0, 0, z);
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    muki = 0;
                }else if (Input.GetKey(KeyCode.DownArrow))
                {
                    muki = 6;
                }
            }
            CmdMove(this.transform.position);
        }
    }

    private void LerpPosition() //補間
    {
        
        Vector3 pos = Vector3.Lerp(transform.position, m_ReceivedPosition, m_LerpRate * Time.deltaTime);
        
        transform.position = pos;
    }

    [Command]
    void CmdMove(Vector3 vec3)
    {
        foreach(var conn in NetworkServer.connections)
        {
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

    [ClientCallback]
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit物1:" + collision.gameObject);
        Debug.Log("hit物2:" + collision.gameObject.ToString());
        //Debug.Log("リス:" + Spawn);
        if (collision.gameObject.tag == "bullet") 
        {
            Debug.Log("リス:" + Spawn);
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

    [Command]
    void CmdFire(int muki)
    {
        //弾の生成
        //発射
        //ネットワークで弾の生成
        //デストロイ
        switch (muki)
        {
            case 0:
                var bullet_f = Instantiate(bulletprehub, transform.position + transform.forward, Quaternion.identity);
                bullet_f.GetComponent<Rigidbody>().velocity = transform.forward * 4;
                NetworkServer.Spawn(bullet_f);
                Destroy(bullet_f, 2.0f); break;
            case 3:
                var bullet_r = Instantiate(bulletprehub, this.transform.position + transform.right, Quaternion.identity);
                bullet_r.GetComponent<Rigidbody>().velocity = transform.right * 4;
                NetworkServer.Spawn(bullet_r);
                Destroy(bullet_r, 2.0f); break;
            case 6:
                var bullet = Instantiate(bulletprehub, transform.position - transform.forward, Quaternion.identity);
                bullet.GetComponent<Rigidbody>().velocity = -transform.forward * 4;
                NetworkServer.Spawn(bullet);
                Destroy(bullet, 2.0f); break;
            case 9:
                var bullet_l = Instantiate(bulletprehub, transform.position - transform.right, Quaternion.identity);
                bullet_l.GetComponent<Rigidbody>().velocity = -transform.right * 4;
                NetworkServer.Spawn(bullet_l);
                Destroy(bullet_l, 2.0f); break;
        }
    }
}
