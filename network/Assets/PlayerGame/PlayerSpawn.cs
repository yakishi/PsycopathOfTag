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
        shokihantei = false;
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(this.tag);
	}

    [Command]
    void CmdPlayerSpawn()
    {
        if (shokihantei == false)
        {
            if (playerNetID.Value % 2 == 1)
            {
                spawn = GameObject.Find("red_spawn1");
                transform.Translate(spawn.transform.position);
                team = true;
                //this.tag.
            }
            else
            {
                spawn = GameObject.Find("blue_spawn1");
                transform.Translate(spawn.transform.position);
                team = false;
                //this.tag = "blue";
            }

            transform.Translate(resposition);
            resposition = spawn.transform.position;
            shokihantei = true;
        }
    }

    [Command]
    void CmdRespoawn()
    {
        Debug.Log("ResPOsition" + resposition);
        GetComponent<HealthBar>().TakeDamage(1);
    }

    public Vector3 Respawn()
    {
        CmdRespoawn();
        Debug.Log("Respawn");
        return resposition;
        
    }

    public bool teamhantei()
    {
        return team;
    }
}
