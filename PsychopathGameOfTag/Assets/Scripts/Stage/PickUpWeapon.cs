using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour {

    GameObject player;

    [SerializeField]
    GameObject prefab;

    [SerializeField]
    string id;

    public string getModeID
    {
        get
        {
            return id;
        }
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(new Vector3(0, 1, 0));
    }

    private void OnTriggerEnter (Collider collider)
    {
        if (collider.gameObject.tag != "Player"
                || collider.GetComponent<Player>().Type == Player.PlayerMode.Chase) {
            return;
        }

        foreach (Transform n in collider.transform) {
            GameObject child = n.gameObject;
            if(child.tag == "Weapon" && this.tag == "Weapon") {
                Destroy(n.gameObject);
            }
        }


        if (this.tag == "Trap") {
            EscapePlayer escapePlayer = collider.gameObject.GetComponent<EscapePlayer>();
            escapePlayer.trapId = id;
            escapePlayer.setTrapPrefab = prefab;
        }
        else if (this.tag == "Weapon") {
            Instantiate(prefab, collider.transform);

            foreach (Transform n in collider.transform) {
                GameObject child = n.gameObject;
                if (child.tag == "Weapon") {
                    n.gameObject.transform.position = n.parent.transform.position + Vector3.up * 3.0f;
                }
            }
        }

        Destroy(gameObject);
        
    }

}
