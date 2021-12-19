using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Networking;

public class NFT_Render : MonoBehaviour
{
    // Start is called before the first frame update
    public string tokenAddress;
    public int tokenId;
    public string blockMinted;
    public string tokenURI;
    public string metadata;
    public string Amount;
    public string nameOfNFT;
    public string Symbol;

    public SpriteRenderer spriteRenderer;

    private Sprite targetSprite;

    private void Start()
    {
        StartCoroutine(GetTextureRequest(tokenURI, (response) => {
            targetSprite = response;
            spriteRenderer.sprite = targetSprite;
        }));
    }

    IEnumerator GetTextureRequest(string url, System.Action<Sprite> callback)
    {
        using (var www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    var texture = DownloadHandlerTexture.GetContent(www);
                    var rect = new Rect(0, 0, 600f, 600f);
                    var sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
                    callback(sprite);
                }
            }
        }
    }
}
