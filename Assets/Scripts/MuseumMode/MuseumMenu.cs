using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumMenu : MonoBehaviour
{
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
    }

}
