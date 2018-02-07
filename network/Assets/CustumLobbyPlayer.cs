using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class CustumLobbyPlayer : NetworkLobbyPlayer {

    private NetworkInstanceId netID;
    private GameObject player;

    Text PlayerState;

    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        this.readyToBegin = false;
        netID = GetComponent<NetworkIdentity>().netId;
        PlayerState = GameObject.Find("PlayerStateText").GetComponent<Text>();
    }

    //private void Start()
    //{
    //    this.readyToBegin = false;
    //    Counter = GameObject.Find("PlayerCountObject").GetComponent<PlayerCount>();
    //    netID = GetComponent<NetworkIdentity>().netId;
    //}


    private void Update()
    {
        
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            StateChange();
        }

        if (!this.readyToBegin)
        {
            PlayerState.text = "準備中";
        }
        else
        {
            PlayerState.text = "準備完了";
        }
    }
    public override void OnClientReady(bool readyState) //これでゲームの開始の判定をする
    {
        DontDestroyOnLoad(gameObject);  //偏移時に破棄されているからこれを残す
        base.OnClientReady(readyState);
        Debug.Log("readystate:" + readyState);
        Debug.Log("readytobegin:" + this.readyToBegin);
    }

    public void Ready()     //ボタン押したら準備状態の変化
    {
        if (isLocalPlayer)
        {
            if (this.readyToBegin)
            {
                Debug.Log("false");
                this.ShowLobbyGUI = false;
                this.readyToBegin = false;
            }
            else
            {
                Debug.Log("true");
                this.ShowLobbyGUI = true;
                this.readyToBegin = true;
            }
        }
        
    }

    public void StateChange()      //プレイヤーの準備状態の変化
    {
        player = GameObject.Find("Player" + netID);
        ReadyStateButton();
    }

    public void ReadyStateButton()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (this.readyToBegin == true)
        {
            this.readyToBegin = false;
            
            SendNotReadyToBeginMessage();
        }
        else
        {
            this.readyToBegin = true;
            SendReadyToBeginMessage();
        }        
    }
}
