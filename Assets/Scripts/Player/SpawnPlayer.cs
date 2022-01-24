using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : NetworkBehaviour
{
    public GameObject playerPrefab;
    public string walletaddress;
    bool isAuthenticated = false;

    #region server

    public override void OnStartServer()
    {
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region client
    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
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

            walletaddress = string.Format("{0}...{1}", addr.Substring(0, 6), addr.Substring(addr.Length - 3, 3));

            isAuthenticated = true;

            Debug.Log(walletaddress);
        }
    }
}
