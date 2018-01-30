using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChase : Player {

    [SerializeField]
    GameObject prefab;

    bool fire;
	// Use this for initialization
	void Start () {
        Initialize();
        fire = false;
	}
	
	// Update is called once per frame
	public override void FixedUpdate () {
        type = PlayerMode.Chase;
        if (MyInput.OnTrigger() && !fire) {
            GameObject bullet = GameObject.Instantiate(prefab, this.transform.position + Vector3.forward * 1.2f, Quaternion.identity);
            bullet.AddComponent<Bullet>();

            //anim.PlayAnimation("Attack1Trigger");
            fire = true;
        }
        else if(MyInput.TriggerAxis() == 0.0f) {
            //anim.StopAnimation("Attack1Trigger");
            fire = false;
        }
        base.FixedUpdate();
	}
}
