using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] public GameObject lobbyUI = null;
    [SerializeField] public Button startGameButton = null;

    private void Start()
    {
        BlockchainNetworkManager.ClientOnConnected += HandleClientConnected;
    }

    public void StartGame()
    {
        NetworkClient.connection.identity.GetComponent<SpawnPlayer>().CmdStartGame();
    }

    private void OnDestroy()
    {
        BlockchainNetworkManager.ClientOnConnected -= HandleClientConnected;
    }

    private void HandleClientConnected()
    {
        lobbyUI.SetActive(true);
    }

    public void LeaveLobby()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
            SceneManager.LoadScene(0);
        }
    }

}
