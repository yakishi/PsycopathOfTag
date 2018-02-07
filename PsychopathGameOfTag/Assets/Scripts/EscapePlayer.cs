using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EscapePlayer : NetworkBehaviour {

    Player player;

    private GameObject trapPrefab;
    public GameObject setTrapPrefab
    {
        set
        {
            trapPrefab = value;
        }
    }

    public string trapId;

    public override void OnStartLocalPlayer()
    {       
        StartEscapePlayer();

        base.OnStartLocalPlayer();
    }

    public void StartEscapePlayer()
    {
        trapId = "";
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    public void Update () {

        if (!isLocalPlayer) return;

        if (player.isDead) {
            if (!player.pointFlag) {
                player.CmdAddPoint(player.EnemyTeam(),5);
                player.pointFlag = true;
            }
            player.DeadTime();
        }

        
        if (MyInput.OnTrigger() && trapId != "") {
            trapPrefab.GetComponent<Trap>().ID = trapId;
            trapPrefab.GetComponent<Trap>().SetPlayerTeamInfo(player.team);
            Instantiate(trapPrefab, new Vector3(transform.position.x + -player.moveForward.x, -0.5f, transform.position.z + -player.moveForward.z), Quaternion.identity);

            ClearTrapInfo();
        }

    }

    void ClearTrapInfo()
    {
        trapId = "";
        trapPrefab = null;
    }


   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") {

            Player colPlayer = collision.gameObject.GetComponent<Player>();

            if (player.Type == Player.PlayerMode.Chase && colPlayer.team == player.team) {

                ChasePlayer chaser = collision.gameObject.GetComponent<ChasePlayer>();
                foreach (Transform n in this.gameObject.transform) {
                    if (n.gameObject.tag == "Weapon") {
                        chaser.Change_Mode(n.GetComponent<WeaponType>().getModeID);
                        DestroyObject(n.gameObject);
                    }
                }
            }
        }
    }
}
