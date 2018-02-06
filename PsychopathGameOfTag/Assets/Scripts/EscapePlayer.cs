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

    public override void Start()
    {
        type = PlayerMode.Escape;
        trapId = "";
        base.Start();

    }

    // Use this for initialization
    //public override void OnStartServer () {
    //    base.OnStartServer();
    //    Initialize();
    //    type = PlayerMode.Escape;
    //    trapId = "";
    //}

    public override void ChangeType(Player p)
    {
        base.ChangeType(p);
    }

    // Update is called once per frame
    public override void FixedUpdate () {
        

        if (MyInput.OnTrigger() && trapId != "") {
            trapPrefab.GetComponent<Trap>().ID = trapId;
            trapPrefab.GetComponent<Trap>().SetPlayerTeamInfo(team);
            Instantiate(trapPrefab, new Vector3(transform.position.x + -moveForward.x, -0.5f, transform.position.z + -moveForward.z), Quaternion.identity);

            ClearTrapInfo();
        }

        
        if (isDead) {
            if (!pointFlag) {
                CmdAddPoint();
            }
            pointFlag = true;
            DeadTime();
        }

        base.FixedUpdate();
    }

    void ClearTrapInfo()
    {
        trapId = "";
        trapPrefab = null;
    }

    [Command]
    public override void CmdAddPoint()
    {
        game.CmdAddPoint(EnemyTeam(), 5);
        base.CmdAddPoint();
    }

    Game.Team EnemyTeam()
    {
        Game.Team temp = team;
        switch (team) {
            case Game.Team.red:
                temp = Game.Team.blue;
                break;
            case Game.Team.blue:
                temp = Game.Team.red;
                break;
        }

        return temp;
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
