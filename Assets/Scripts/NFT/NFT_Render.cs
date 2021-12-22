using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
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
    public string name;
    public string Symbol;
    public TMP_Text addressText;
    public TMP_Text tokenIdText;
    public TMP_Text tokenNameText;

    public SpriteRenderer spriteRenderer;
    private Sprite targetSprite;

    public GameObject frame;

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
                    var rect = new Rect(0, 0, texture.width,texture.height);
                    var sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
                    
                    transform.localScale -= new Vector3(0.75f, 0.75f, 0.75f);

                    transform.position = (transform.position + new Vector3(0, 3 + texture.height / 1000 , 0));

                    GameObject NFTframe = Instantiate(frame, this.transform);
                    NFTframe.transform.localScale = new Vector3(texture.width/100 + 1, texture.height/100 + 1,.1f);
                    
                   
                    callback(sprite);
                    addressText.SetText("Tokken Address: \n" + tokenAddress);
                    addressText.transform.position += new Vector3(0, -NFTframe.transform.localScale.y * 0.15f, 0);
                    tokenIdText.SetText("Token Id: " + tokenId);
                    tokenIdText.transform.position += new Vector3(0, -NFTframe.transform.localScale.y * 0.15f, 0);
                    tokenNameText.SetText("Token Name: " + name);
                    tokenNameText.transform.position += new Vector3(0, -NFTframe.transform.localScale.y * 0.15f, 0);
                }
            }
        }
    }
}
