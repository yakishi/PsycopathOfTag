using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustumNetworkManager : NetworkManager {

    public override NetworkClient StartHost()
    {
        Debug.Log("StartHost");
        return base.StartHost();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        Debug.Log("OnserverConnect " + conn.connectionId);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.Log("OnclientConnect " + conn.connectionId);
    }

    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
        Debug.Log("OnSartClient");
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("OnServerAddPlayer" + conn.connectionId + ":" + playerControllerId);
        base.OnServerAddPlayer(conn, playerControllerId);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        Debug.Log("OnSceanChanged " + sceneName);
        base.OnServerSceneChanged(sceneName);
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        Debug.Log("OnClientSceanChanged " + conn.connectionId);
        base.OnClientSceneChanged(conn);
    }
}
