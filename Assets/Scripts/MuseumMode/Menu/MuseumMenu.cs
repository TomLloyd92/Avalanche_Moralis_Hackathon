using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MuseumMenu : NetworkBehaviour
{
    [SerializeField] private TMP_InputField contractAddressInput = null;
    string address = "";


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(isServer)
        {
            if(((BlockchainNetworkManager)NetworkManager.singleton).museumMode == true)
            {
                ((BlockchainNetworkManager)NetworkManager.singleton).museumMode = true;
                ((BlockchainNetworkManager)NetworkManager.singleton).museumContractAddress = contractAddressInput.text;
                address = contractAddressInput.text;
                RpcMuseumMode(address);
            }

        }
    }

    public void museumModeActive()
    {
        ((BlockchainNetworkManager)NetworkManager.singleton).museumMode = true;
        ((BlockchainNetworkManager)NetworkManager.singleton).museumContractAddress = contractAddressInput.text;
        RpcMuseumMode(contractAddressInput.text);
    }

    [ClientRpc]
    void RpcMuseumMode(string newAddress)
    {
        ((BlockchainNetworkManager)NetworkManager.singleton).museumMode = true;
        ((BlockchainNetworkManager)NetworkManager.singleton).museumContractAddress = newAddress;
    }

}
