using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TimeGame : NetworkBehaviour {

    Text text;
    public int timelimit;
    private static System.DateTime startTime = System.DateTime.Now;
    string seconds;

    public override void OnStartServer()
    {
        startTime = System.DateTime.Now;
    }

    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            text = GameObject.Find("Time").GetComponent<Text>();
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
        int count = timelimit - (int)((System.DateTime.Now - startTime).TotalSeconds);

        RpcSetCount(count);
    }

    [ClientRpc]
    void RpcSetCount(int n)
    {
        seconds = (n % 60).ToString();
        if (n%60 < 10)
        {
            seconds = "0" + (n % 60).ToString();
        }

        if (text != null)
        {
            text.text = n / 60 + ":" + seconds;
        }
    }
}
