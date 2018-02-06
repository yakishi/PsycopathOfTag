using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSpawn :  NetworkBehaviour{

    GameObject spawn;
    GameObject count_obj;
    PlayerSpawn m_Player;
    public Vector3 resposition;
    private NetworkInstanceId playerNetID;

    bool team;
    bool shokihantei;


    public override void OnStartLocalPlayer()
    {
        playerNetID = GetComponent<NetworkIdentity>().netId;
        m_Player = this.gameObject.GetComponent<PlayerSpawn>();
        CmdPlayerSpawn();
        Debug.Log(spawn.transform.position);
        shokihantei = false;
    }
	
	// Update is called once per frame
	void Update () {
	}

    
    void CmdPlayerSpawn()
    {
        //if (shokihantei == false)
        //{
            if (playerNetID.Value % 2 == 1)
            {
                spawn = GameObject.Find("bule_spawn1");
                transform.Translate(spawn.transform.position);
                team = true;
            }
            else
            {
                spawn = GameObject.Find("red_spawn1");
                transform.Translate(spawn.transform.position);
                team = false;
            }
        //}
    }

    public bool teamhantei()
    {
        return team;
    }
}
