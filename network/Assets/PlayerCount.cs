using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerCount : NetworkBehaviour {

    [SyncVar(hook ="textsync")]int P_Count;

    Text Count;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        //P_Count = 0;
        Count = GameObject.Find("PlayerCount").GetComponent<Text>();
        //Count.text = P_Count.ToString();
	}

    [ServerCallback]
    void Update()
    {
           // RpcTextSync(P_Count);
             if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log(Count);
        }
    }

    [ClientRpc]
    public void RpcTextSync(int count)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        Debug.Log("スクリプトのカウント" + P_Count);
        }
        //Debug.Log("aaaaaa:" + count);
        Count.text = count.ToString();
    }

    [Command]
    public void CmdCountUP()
    {
        P_Count += 1;
        //if (isServer)
        //{
        //    RpcTextSync(P_Count);
        //}
        
    }

    public void count()
    {
        CmdCountUP();
    }

    [Client]
    void textsync(int count)
    {
        Debug.Log("yobidasita");
        Count.text = count.ToString();
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
