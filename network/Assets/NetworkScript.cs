using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkScript : NetworkBehaviour {

    public Canvas canvas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void OnHostButton()
    {
        canvas.gameObject.SetActive(false);
        NetworkManager.singleton.StartHost();
    }

    public void OnClientButton()
    {
        canvas.gameObject.SetActive(false);
        NetworkClient client = NetworkManager.singleton.StartClient();
        Debug.Log(client.serverIp);
    }

    public void OnServerButton()
    {
        canvas.gameObject.SetActive(false);
        NetworkManager.singleton.StartServer();
    }
}
