using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    Player player;
    Vector3 direction;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<Player>();
        direction = player.cameraForward;
	}
	
	// Update is called once per frame
	void Update () {
        direction.Normalize();

        transform.position = transform.position + direction * 1.2f;
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "field") {
            DestroyObject(this.gameObject);
        }

        if (collision.gameObject.tag != "Player") return;

        Player collisionPlayer = collision.gameObject.GetComponent<Player>();

        if (collisionPlayer.team != player.team && !collisionPlayer.isDead && collisionPlayer.Type == Player.PlayerMode.Escape) {
            //collisionPlayer.HitBullet();
        }
    }
}
