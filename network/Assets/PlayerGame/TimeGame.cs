using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TimeGame : NetworkBehaviour {

    Text text;

    private static System.DateTime startTime = System.DateTime.Now;

    public override void OnStartServer()
    {
        startTime = System.DateTime.Now;
    }

    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            text = GameObject.Find("MsgText").GetComponent<Text>();
        }
    }
    // Update is called once per frame
    void Update () {
        if (isServer)
        {
            Count();
        }
    }

    [ServerCallback]
    void Count()
    {
        if (!isServer)
        {
            return;
        }
        int count = 600 - (int)((System.DateTime.Now - startTime).TotalSeconds);

        RpcSetCount(count);
    }

    [ClientRpc]
    void RpcSetCount(int n)
    {
        if (text != null)
        {
            text.text = "Time: " + n;
        }
    }
}
