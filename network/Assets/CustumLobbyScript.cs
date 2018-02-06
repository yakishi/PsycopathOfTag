using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;
using System;

public class CustumLobbyScript : NetworkLobbyManager {
    public PlayerCount PC_cs;
    GameObject netPlayer;
    NetworkLobbyPlayer player;
    NetworkLobbyManager NLM;
    int stage1, stage2, stage3;
    bool StageSelect;

    Text PC;

    private void Start()
    {
        StartMatchMaker();
        //NetworkManager.singleton.StartMatchMaker();
        //this.playScene = "stage_o";  <-ステージ決定に使う
        player = lobbyPlayerPrefab.GetComponent<CustumLobbyPlayer>();
        NLM = gameObject.GetComponent<NetworkLobbyManager>();
        StageSelect = false;
        this.showLobbyGUI = false;
    }

    //クライアント関連の関数
    public override void OnStartHost()
    {
        base.OnStartHost();
        //PC_cs = GameObject.Find("PlayerCountObject").GetComponent<PlayerCount>();
        stage1 = stage2 = stage3 = 0;
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        //PlayerCount++;
        netPlayer = GameObject.Find("Player"+ (conn.connectionId + 1));
        NetworkLobbyPlayer hoge =  lobbySlots[conn.connectionId];
    }

    public override void OnLobbyClientEnter()
    {
        Debug.Log("入室");
        base.OnLobbyClientEnter();
        //PC_cs = GameObject.Find("PlayerCountObject").GetComponent<PlayerCount>();
        //PC = GameObject.Find("PlayerCountText").GetComponent<Text>();
        //PC_cs.CmdCountUP();
        //Debug.Log(PC_cs.CountReturn());
    }

    public override void OnLobbyStopClient()
    {
        Debug.Log("退室2");
        base.OnLobbyStopClient();
    }


    public override void OnLobbyServerConnect(NetworkConnection conn)
    {
        base.OnLobbyServerConnect(conn);
        PC = GameObject.Find("PlayerCountText").GetComponent<Text>();
        PC_cs.count();
        PC_cs.textsync(PC_cs.CountReturn());
    }

    public override void OnLobbyServerDisconnect(NetworkConnection conn)
    {
        Debug.Log("削除");
        PC_cs.CountDown();
        base.OnLobbyServerDisconnect(conn);
    }
    public override void OnLobbyClientSceneChanged(NetworkConnection conn)
    {
        base.OnLobbyClientSceneChanged(conn);
        
    }

    //プレイヤーの処理
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("RTT:" + NLM.client.GetRTT().ToString()); //1/1000秒の往復遅延時間
            if (this.showLobbyGUI == true)
            {
                this.showLobbyGUI = false;
            }
            else
            {
                this.showLobbyGUI = true;
            }
        }else if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log(PC_cs.CountReturn());
            player.OnClientReady(true);
        }
    }

    public void Clientstop()
    {
        //NetworkManager.singleton.StopMatchMaker();
        //OnLobbyStopHost();
        //StopMatchMaker();
        //StopServer();
        OnLobbyStopClient();
        OnLobbyClientExit();
    }

    public override void OnLobbyStopHost()
    {
        Debug.Log("StopHost");
        base.OnLobbyStopHost();
    }

    public void LoadScean(int stagenum)
    {
        switch (stagenum)
        {
            case 1: StageCount(1);break;
            case 2: StageCount(2);break;
            case 3: StageCount(3);break;
        }
    }

    void StageCount(int n)
    {
        if (StageSelect == true)
        {
            StageSelect = false;
            stage1 = stage2 = stage3 = 0;
        }
        else
        {
            switch (n)
            {
                case 1: stage1++; break;
                case 2: stage2++; break;
                case 3: stage3++; break;
            }
            StageSelect = true;
        }
        Debug.Log("stage1:2:3:" + stage1 + "," + stage2 + "," + stage3);
    }

}
