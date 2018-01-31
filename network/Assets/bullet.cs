using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class bullet : NetworkBehaviour {

    
    private NetworkInstanceId playerNetID;
    public override void OnStartLocalPlayer()
    {
    }

    private void Update()
    {
        //player = GameObject.Find("Player").GetComponent<PlayerSpawn>();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    playerNetID = GetComponent<NetworkIdentity>().netId;
    //    var hit = collision.gameObject;
    //    //playerNetID = collision.GetComponent<NetworkIdentity>.netId();
    //    var hitPlayer = hit.GetComponent<PlayerMove>();
    //    Debug.Log("aaa:"+ collision.gameObject);
    //    Debug.Log("spawn:" + collision.gameObject.GetComponent<PlayerSpawn>().Respawn());

    //    Destroy(this.gameObject);
    //    //this.GetComponent<NetworkIdentity>().RemoveClientAuthority(this.connectionToClient);

    //    if (hitPlayer != null)
    //    {
    //        var damage = hit.GetComponent<HealthBar>();
    //        Debug.Log("hit");
    //        damage.TakeDamage(10);
    //        collision.gameObject.GetComponent<PlayerSpawn>().Respawn();
    //        //Debug.Log(collision.gameObject.GetComponent<PlayerSpawn>().teamhantei());
    //    }
    //}

    public void TakeDamage(Collision colission)
    {
        //if (!isServer)
        //{
        //    return;
        //}
        Debug.Log("Dead!");
        //colission.transform.position;
    }
}
