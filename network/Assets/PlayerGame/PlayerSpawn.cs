using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSpawn :  NetworkBehaviour{

    GameObject spawn;
    PlayerSpawn m_Player;
    private NetworkInstanceId playerNetID;

    public override void OnStartLocalPlayer()
    {
        playerNetID = GetComponent<NetworkIdentity>().netId;
        m_Player = this.gameObject.GetComponent<PlayerSpawn>();
        Debug.Log(m_Player);
        CmdPlayerSpawn();
    }
	
	// Update is called once per frame
	void Update () {
	}

    
    void CmdPlayerSpawn()
    {
        if (playerNetID.Value % 2 == 1)
        {
            spawn = GameObject.Find("red_spawn1");
            transform.Translate(spawn.transform.position);
            this.GetComponent<Player>().team = Game.Team.red;
            if (playerNetID.Value % 4 == 3)
            {
                this.GetComponent<Player>().type = Player.PlayerMode.Chase;
                Debug.Log("3で" + this.GetComponent<Player>().type);
            }
            else
            {
                this.GetComponent<Player>().type = Player.PlayerMode.Escape;
                Debug.Log("1で" + this.GetComponent<Player>().type);
            }
        }
        else
        {
            spawn = GameObject.Find("blue_spawn1");
            transform.Translate(spawn.transform.position);
            this.GetComponent<Player>().team = Game.Team.blue;
            if (playerNetID.Value % 4 == 2)
            {
                this.GetComponent<Player>().type = Player.PlayerMode.Chase;
                Debug.Log("2で" + this.GetComponent<Player>().type);
            }
            else
            {
                this.GetComponent<Player>().type = Player.PlayerMode.Escape;
                Debug.Log("4で" + this.GetComponent<Player>().type);
            }
        }
    }
}
