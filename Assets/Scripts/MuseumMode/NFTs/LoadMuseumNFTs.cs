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
    public int ChainId;
    private bool tokensLoaded;
    private bool blocksLoaded;

    //Block
    [SerializeField]
    private List<MuseumBlock> museumBlocksInstantiated;
    public GameObject museumBlock;
    public Transform[] transforms;

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

            NftCollection collectionNFTs = MoralisInterface.GetClient().Web3Api.Token.GetAllTokenIds("0x9b40972E8b1EcAF1B4b9E015AAD33cF04B3626D2", (ChainList)ChainId);
            Debug.Log(collectionNFTs.Result.Count);

            //CREATE THE NECESSARY MUSEUM BLOCKS
            int amountBlocks = (collectionNFTs.Result.Count) / 4;
            //float amountBlockRowsAndColums = Mathf.Ceil( Mathf.Sqrt(amountBlocks));

            for (int i = 0; i < amountBlocks; i++)
            {
                GameObject newblock = Instantiate(museumBlock, new Vector3(i * 25, 0, 0), Quaternion.identity);
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

            NftCollection collectionNFTs = MoralisInterface.GetClient().Web3Api.Token.GetAllTokenIds("0x9b40972E8b1EcAF1B4b9E015AAD33cF04B3626D2", (ChainList)ChainId);
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
  

                currentNFT++;
            }





            yield return 0;
            //foreach (NftCollection token in tokens)
            //{
            //    // Ignor entry without symbol
            //    if (string.IsNullOrWhiteSpace(token.Symbol))
            //    {
            //        continue;
            //    }

            //    // Create and add an Token button to the display list. 
            //    var tokenObj = Instantiate(ListItemPrefab, TokenListTransform);
            //    var tokenSymbol = tokenObj.GetFirstChildComponentByName<Text>("TokenSymbolText", false);
            //    var tokenBalanace = tokenObj.GetFirstChildComponentByName<Text>("TokenCountText", false);
            //    var tokenImage = tokenObj.GetFirstChildComponentByName<Image>("TokenThumbNail", false);
            //    var tokenButton = tokenObj.GetComponent<Button>();
            //    var rectTransform = tokenObj.GetComponent<RectTransform>();

            //    var parentTransform = TokenListTransform.GetComponent<RectTransform>();
            //    double balance = 0.0;
            //    float tokenDecimals = 18.0f;

            //    // Make sure a response to the balanace request weas received. The 
            //    // IsNullOrWhitespace check may not be necessary ...
            //    if (token != null && !string.IsNullOrWhiteSpace(token.Balance))
            //    {
            //        double.TryParse(token.Balance, out balance);
            //        float.TryParse(token.Decimals, out tokenDecimals);
            //    }

            //    tokenSymbol.text = token.Symbol;
            //    tokenBalanace.text = string.Format("{0:0.##} ", balance / (double)Mathf.Pow(10.0f, tokenDecimals));

            //    // When button clicked display theCoingecko page for that token.
            //    tokenButton.onClick.AddListener(delegate
            //    {
            //        // Display token CoinGecko page on click.
            //        Application.OpenURL($"https://coinmarketcap.com/currencies/{token.Name}");
            //    });

            //    using (UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(token.Thumbnail))
            //    {
            //        yield return imageRequest.SendWebRequest();

            //        if (imageRequest.isNetworkError)
            //        {
            //            Debug.Log("Error Getting Nft Image: " + imageRequest.error);
            //        }
            //        else
            //        {
            //            Texture2D tokenTexture = ((DownloadHandlerTexture)imageRequest.downloadHandler).texture;

            //            var sprite = Sprite.Create(tokenTexture,
            //                        new Rect(0.0f, 0.0f, tokenTexture.width, tokenTexture.height),
            //                        new Vector2(0.75f, 0.75f), 100.0f);

            //            tokenImage.sprite = sprite;
            //        }
            //    }

        }
        yield return 0;
    }
}
