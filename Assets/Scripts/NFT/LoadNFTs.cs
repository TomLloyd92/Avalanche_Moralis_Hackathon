using Moralis.Platform.Objects;
using Moralis.Web3Api.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using UnityEngine.Networking;
using Mirror;

public class LoadNFTs : NetworkBehaviour
{

    //await Moralis.Plugins.covalent.getErc20TokenTransfersForAddress(GetErc20TokenTransactionsForAddressDto);


    /// <summary>
    /// Chain ID to fetch tokens from. Might be better to make this
    /// a drop down that is selectable at run time.
    /// </summary>
    public int ChainId;
    private bool tokensLoaded;
    [SerializeField]
    private GameObject NFT;
    private int currentPicture;
    public Transform[] transforms;

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
        PopulateWallet();
    }

    public void PopulateWallet()
    {
        if (!tokensLoaded && isServer)
        {
            StartCoroutine(BuildTokenList());

            // Make sure that duplicate tokens are not loaded.
            tokensLoaded = true;
        }
    }

    IEnumerator BuildTokenList()
    {
        // Get user object and display user name
        MoralisUser user = MoralisInterface.GetUser();

        if (user != null)
        {
            string addr = user.authData["moralisEth"]["id"].ToString();

            NftOwnerCollection tokens = MoralisInterface.GetClient().Web3Api.Account.GetNFTs(addr.ToLower(), (ChainList)ChainId);
            
            Debug.Log("THE FOLLOWING ARE THE NFTS LOADING: ");
            Debug.Log(tokens.Result.Count);

            foreach(NftOwner token in tokens.Result)
            {

                GameObject nft = Instantiate(NFT, transforms[currentPicture]);
                nft.GetComponent<NFT_Render>().name = token.Name;
                nft.GetComponent<NFT_Render>().tokenAddress = token.TokenAddress;
                nft.GetComponent<NFT_Render>().tokenURI = token.TokenUri;


                currentPicture++;
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

