using Moralis.Platform.Objects;
using Moralis.Web3Api.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using UnityEngine.Networking;
using Mirror;

public class LoadMuseumNFTs : NetworkBehaviour
{
    //ID + Bools
    //43113 Testnet
    //43114 Avalanche Net
    public int ChainId;
    private bool tokensLoaded;
    private bool blocksLoaded;

    //Wall
    public GameObject outsideWall;
    public GameObject outSideWallSide;
    private GameObject[] allWalls;
    private int currentWalls = 1;
    private int wallX = 0;
    private int wallZ = 0;


    //Block
    [SerializeField]
    private List<MuseumBlock> museumBlocksInstantiated;
    public GameObject museumBlock;
    public Transform[] transforms;
    private int rowBlock = 0;

    //NFTs
    [SerializeField]
    private GameObject NFT;
    private int currentNFT = 0;
    private int currentBlock = 0;
    


    #region Server
    [Command]
    private void CmdSpawnNFTs()
    {

    }
    #endregion

    #region Client

    #endregion


    private void Update()
    {
        PopulateBlock();

        PopulateMuseum();
    }

    public void PopulateBlock()
    {
        if (!blocksLoaded)
        {
            StartCoroutine(BuildBlock());

            // Make sure that duplicate tokens are not loaded.
      
        }
    }

    public void PopulateMuseum()
    {
        if (!tokensLoaded && blocksLoaded)
        {
            StartCoroutine(BuildTokenList());

            // Make sure that duplicate tokens are not loaded.
            tokensLoaded = true;
        }
    }

    IEnumerator BuildBlock()
    {
        // Get user object and display user name
        MoralisUser user = MoralisInterface.GetUser();

        if (user != null)
        {
            string addr = user.authData["moralisEth"]["id"].ToString();

            NftCollection collectionNFTs = MoralisInterface.GetClient().Web3Api.Token.GetAllTokenIds(((BlockchainNetworkManager)NetworkManager.singleton).museumContractAddress.ToLower(), (ChainList)ChainId);

            //CREATE THE NECESSARY MUSEUM BLOCKS
            float amountBlocks = Mathf.Ceil((collectionNFTs.Result.Count) / 4.0f);
            float amountBlockRowsAndColums = Mathf.Ceil( Mathf.Sqrt(amountBlocks));

            //OUTER WALLS

            for(int i =0; i < amountBlockRowsAndColums ; i++)
            {
                GameObject newblock = Instantiate(outsideWall, new Vector3(-5 , 0, (i * 20) -5 ), Quaternion.Euler(new Vector3(0, currentWalls * 90, 0)));
            }
            currentWalls++;

            for (int i = 0; i < amountBlockRowsAndColums; i++)
            {
                GameObject newblock = Instantiate(outSideWallSide, new Vector3((i * 25) -5, 0,amountBlockRowsAndColums * 15 ), Quaternion.Euler(new Vector3(0, currentWalls * 90, 0)));
            }
            currentWalls++;
            for (int i = 0; i < amountBlockRowsAndColums ; i++)
            {
                GameObject newblock = Instantiate(outsideWall, new Vector3(-5 + (amountBlockRowsAndColums * 25), 0, (i * 20) + 15), Quaternion.Euler(new Vector3(0, currentWalls * 90, 0)));
            }
            currentWalls++;
            for (int i = 0; i < amountBlockRowsAndColums; i++)
            {
                GameObject newblock = Instantiate(outSideWallSide, new Vector3((i * 25)+ 20, 0, -5), Quaternion.Euler(new Vector3(0, currentWalls * 90, 0)));
            }

            for (int i = 0; i < amountBlocks; i++)
            {

                if (i != 0 && i % amountBlockRowsAndColums == 0)
                {
                    rowBlock++;
                }

                GameObject newblock = Instantiate(museumBlock, new Vector3((i % amountBlockRowsAndColums) * 25, 0, rowBlock * 20), Quaternion.identity);
                museumBlocksInstantiated.Add(newblock.GetComponent<MuseumBlock>());

    

                if(i == amountBlocks - 1)
                {
                    blocksLoaded = true;
                }
            }



        }
        yield return 0;
    }

    IEnumerator BuildTokenList()
    {
        // Get user object and display user name
        MoralisUser user = MoralisInterface.GetUser();

        if (user != null)
        {
            string addr = user.authData["moralisEth"]["id"].ToString();

            NftCollection collectionNFTs = MoralisInterface.GetClient().Web3Api.Token.GetAllTokenIds(((BlockchainNetworkManager)NetworkManager.singleton).museumContractAddress.ToLower(), (ChainList)ChainId);
            Debug.Log(collectionNFTs.Result.Count);

            //Pupulate NFTs
            foreach (Nft token in collectionNFTs.Result)
            {

            
                if (currentNFT == 4)
                {
                    currentBlock++;
                    currentNFT = 0;
                }



                GameObject nft = Instantiate(NFT, museumBlocksInstantiated[currentBlock].GetComponent<MuseumBlock>().displayPositions[currentNFT].position, Quaternion.Euler(new Vector3(0, currentNFT * 90, 0)));

                nft.GetComponent<NFT_Render>().name = token.Name;
                nft.GetComponent<NFT_Render>().tokenAddress = token.TokenAddress;
                nft.GetComponent<NFT_Render>().tokenURI = token.TokenUri;
                nft.GetComponent<NFT_Render>().tokenId = int.Parse(token.TokenId);

                //Debug.Log(token.TokenUri);
       

                currentNFT++;
            }

            yield return 0;

        }
        yield return 0;
    }
}
