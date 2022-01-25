using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] public GameObject lobbyUI = null;
    [SerializeField] public Button startGameButton = null;
    [SerializeField] public TMP_Text[] playerNameTexts = new TMP_Text[8];

    private void Start()
    {
        BlockchainNetworkManager.ClientOnConnected += HandleClientConnected;
        SpawnPlayer.ClientOnInfoUpdated += ClientHandleInfoUpdated;
    }

    public void StartGame()
    {
        NetworkClient.connection.identity.GetComponent<SpawnPlayer>().CmdStartGame();
    }

    private void OnDestroy()
    {
        BlockchainNetworkManager.ClientOnConnected -= HandleClientConnected;
        SpawnPlayer.ClientOnInfoUpdated -= ClientHandleInfoUpdated;
    }

    private void ClientHandleInfoUpdated()
    {
        List<SpawnPlayer> players = ((BlockchainNetworkManager)NetworkManager.singleton).Players;

        for (int i = 0; i < players.Count; i++)
        {
            playerNameTexts[i].text = players[i].GetDisplayName();
        }

        for(int i = players.Count; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting For Player...";
        }

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
