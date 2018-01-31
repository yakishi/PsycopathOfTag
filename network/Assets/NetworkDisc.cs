﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkDisc : MonoBehaviour
{

    /** NetworkDiscovery*/
    private NetworkDiscovery netdisc;
    /** NetworkManager*/
    private NetworkManager netman;
    // Use this for initialization
    void Start()
    {
        netman = GetComponent<NetworkManager>();
        netdisc = GetComponent<NetworkDiscovery>();
    }

    // Update is called once per frame
    void Update()
    {
        // NetworkManagerが開始していない時に処理
        if (netdisc.showGUI)
        {
            // NetworkDiscoveryがサーバーとして動作していたら、NetworkManagerをHostで開始する
            if (netdisc.isServer)
            {
                // ホストとして開始
                netman.StartHost();
            }
            // NetworkManagerが開始していたらGUIを消す
            if (netman.isNetworkActive)
            {
                netdisc.showGUI = false;
            }
        }

        if (!netdisc.useNetworkManager)
        {
            netman.OnStartHost();
        }
    }
}
