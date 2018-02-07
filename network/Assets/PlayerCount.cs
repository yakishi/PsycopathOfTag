using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerCount : NetworkBehaviour {

    [SyncVar]int P_Count;

    public Text Count;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        Count = GameObject.Find("PlayerCountText").GetComponent<Text>();
	}

    [ServerCallback]
    void Update()
    {
        RpcTextSync(P_Count);
    }

    [ClientRpc]
    public void RpcTextSync(int count)
    {
        Count.text = count.ToString();
    }

    [Command]
    public void CmdCountUP()
    {
        P_Count += 1;
        RpcTextSync(P_Count);
    }

    public void count()
    {
        P_Count += 1;
    }


    public void textsync(int count)
    {
        if (isServer)
        {
            RpcTextSync(count);
        }
        
    }

    public void CountDown()
    {
        P_Count--;
    }

    public int CountReturn()
    {
        return P_Count;
    }
}
