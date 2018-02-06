using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestChase : Player
{
    // Use this for initialization
    void Start()
    {
        Initialize();
    }

    // Update is calcled once per frame
    public override void FixedUpdate()
    {
        type = PlayerMode.Escape;

        if (isDead) {
            if (!pointFlag) {
                game.AddPoint(EnemyTeam(), 5);
                ReStart();
            }
            pointFlag = true;
        }

        base.FixedUpdate();
    }

    void ReStart()
    {
        hp = 10;
    }
}
