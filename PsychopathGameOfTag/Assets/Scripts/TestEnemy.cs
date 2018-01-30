using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Player {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    public override void FixedUpdate()
    {
        if (isDead) {
            if (!pointFlag) {
                AddPoint();
            }
            pointFlag = true;
            DeadTime();
        }

        return;
        base.FixedUpdate();
    }

    public override void AddPoint()
    {
        game.AddPoint(EnemyTeam(), 5);
        base.AddPoint();
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
}
