using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMikata : Player {

    Player.PlayerMode type;
    public Player.PlayerMode Type
    {
        get
        {
            return type;
        }
    }

	// Use this for initialization
	void Start () {
        Initialize();
        type = PlayerMode.Chase;
	}

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
