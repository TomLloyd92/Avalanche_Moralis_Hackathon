using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MuseumMenu : NetworkBehaviour
{
    [SerializeField] private TMP_InputField contractAddressInput = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void museumModeActive()
    {
        ((BlockchainNetworkManager)NetworkManager.singleton).museumMode = true;
        ((BlockchainNetworkManager)NetworkManager.singleton).museumContractAddress = contractAddressInput.text;
        RpcMuseumMode();
    }

    [ClientRpc]
    void RpcMuseumMode()
    {
        ((BlockchainNetworkManager)NetworkManager.singleton).museumMode = true;
        ((BlockchainNetworkManager)NetworkManager.singleton).museumContractAddress = contractAddressInput.text;
    }

}
