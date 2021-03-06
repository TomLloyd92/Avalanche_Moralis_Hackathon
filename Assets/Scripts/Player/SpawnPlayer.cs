using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : NetworkBehaviour
{
    public GameObject playerPrefab;
    public string walletaddress;
    public string fullWalletAddress;
    bool isAuthenticated = false;
    [SyncVar(hook = nameof(ClientHandleDisplayName))]
    private string displayName;

    public static event Action ClientOnInfoUpdated;

    public string GetDisplayName()
    {
        return displayName;
    }

    #region server
    [Server]
    public void SetDisplayName(string name)
    {
        this.displayName = name;
    }


    public override void OnStartServer()
    {
        DontDestroyOnLoad(gameObject);
    }

    [Command]
    public void CmdStartGame()
    {
        ((BlockchainNetworkManager)NetworkManager.singleton).StartGame();
    }

    #endregion

    #region client

    private void ClientHandleDisplayName(string oldDisplayName, string newDisplayName)
    {
        ClientOnInfoUpdated?.Invoke();
    }

    public override void OnStartClient()
    {
        if(NetworkServer.active)
        {
            return;
        }

        ((BlockchainNetworkManager)NetworkManager.singleton).Players.Add(this);

        DontDestroyOnLoad(gameObject);
    }

    public override void OnStopClient()
    {
        ClientOnInfoUpdated?.Invoke();
        if(!isClientOnly)
        {
            return;
        }

        ((BlockchainNetworkManager)NetworkManager.singleton).Players.Remove(this);

        if(!hasAuthority)
        {
            return;
        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        updateAddress();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void updateAddress()
    {
        // Update character address if it has not been set
        if (!isAuthenticated && MoralisInterface.GetUser() != null)
        {
            string addr = MoralisInterface.GetUser().authData["moralisEth"]["id"].ToString();

            fullWalletAddress = addr;
            walletaddress = string.Format("{0}...{1}", addr.Substring(0, 6), addr.Substring(addr.Length - 3, 3));

            isAuthenticated = true;

            Debug.Log(walletaddress);
        }
    }
}
