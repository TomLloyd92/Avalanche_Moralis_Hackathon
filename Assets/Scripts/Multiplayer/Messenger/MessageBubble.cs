using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageBubble : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer bubbleSprite;
    [SerializeField]
    private TMP_Text textTMP;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        setUp("hello");
    }

    public static void Create(Transform parent, Vector3 localposition, string text)
    {

    }

    public void setUp(string text)
    {
        textTMP.SetText(text);
        //Make sure it updates
        textTMP.ForceMeshUpdate();
        Vector2 textsize = new Vector2 (textTMP.GetRenderedValues(false).x + 50 , textTMP.GetRenderedValues(false).y + 50);
        Vector2 padding = new Vector2(50f, 50f);
        bubbleSprite.size = textsize + padding; 

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
