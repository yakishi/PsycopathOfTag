using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EscapePlayer : Player {

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
        type = PlayerMode.Escape;
        trapId = "";

        base.OnStartLocalPlayer();
    }

    public override void ChangeType(Player p)
    {
        base.ChangeType(p);
    }

    // Update is called once per frame
    public override void Update () {

        if (isDead) {
            if (!pointFlag) {
                CmdAddPoint(EnemyTeam(),5);
            }
            pointFlag = true;
            DeadTime();
        }

        if (!isLocalPlayer) return;
        
        if (MyInput.OnTrigger() && trapId != "") {
            trapPrefab.GetComponent<Trap>().ID = trapId;
            trapPrefab.GetComponent<Trap>().SetPlayerTeamInfo(team);
            Instantiate(trapPrefab, new Vector3(transform.position.x + -moveForward.x, -0.5f, transform.position.z + -moveForward.z), Quaternion.identity);

            ClearTrapInfo();
        }

        

        base.Update();
    }

    void ClearTrapInfo()
    {
        trapId = "";
        trapPrefab = null;
    }


   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") {

            Player player = collision.gameObject.GetComponent<Player>();

            if (player.Type == PlayerMode.Chase && player.team == this.team) {

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
