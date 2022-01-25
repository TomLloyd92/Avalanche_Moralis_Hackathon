using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockchainNetworkManager : NetworkManager
{
    public GameObject playerPre;
    public GameObject camPre;

    public static event Action ClientOnConnected;
    public static event Action ClientOnDisconnected;

    public List<SpawnPlayer> Players { get; } = new List<SpawnPlayer>();

    private bool isGameInProgress = false;

    #region Server

    public override void OnServerSceneChanged(string newSceneName)
    {

        foreach (SpawnPlayer player in Players)
        {

            //GameObject camInstance = Instantiate(camPre, new Vector3(0, 0, 0), Quaternion.identity);
            //NetworkServer.Spawn(camInstance, player.connectionToClient);

            GameObject baseInstance = Instantiate(playerPre, new Vector3(0,0,0), Quaternion.identity);
            NetworkServer.Spawn(baseInstance, player.connectionToClient);


        }



        if (SceneManager.GetActiveScene().name.StartsWith("Demo_Level"))
        {
            Debug.Log("SPAWNING IN THE PLAYER");
       
        }
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);

        if (!isGameInProgress)
        {
            return;
        }

        conn.Disconnect();

    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        SpawnPlayer player = conn.identity.GetComponent<SpawnPlayer>();

        Players.Remove(player);

        base.OnServerDisconnect(conn);
    }

    public void StartGame()
    {
        Debug.Log("Starting Game");
        if (Players.Count < 0)
        {
            return;
        }

        isGameInProgress = true;

        ServerChangeScene("Demo_Level");
    }

    public override void OnStopServer()
    {
        Players.Clear();

        isGameInProgress = false;
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        SpawnPlayer player = conn.identity.GetComponent<SpawnPlayer>();

        Players.Add(player);
    }



    #endregion

    #region Client
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        ClientOnConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        ClientOnDisconnected?.Invoke();
    }

    public override void OnStopClient()
    {
        Players.Clear();
    }

    #endregion

}
