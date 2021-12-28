using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessengerBehaviour : NetworkBehaviour
{
    [SerializeField] 
    private GameObject chatUI = null;

    [SerializeField]
    private TMP_Text chatText = null;
    
    [SerializeField]
    private TMP_InputField inputField = null;

    [SerializeField]
    private GameObject chatBubble;


    private static event Action<string> OnMessage;

    public override void OnStartAuthority()
    {
        chatUI.SetActive(true);

        OnMessage += HandleNewMessage;



    }

    [ClientCallback]
    private void OnDestroy()
    {
        if (!hasAuthority)
        {
            return;
        }

        OnMessage -= HandleNewMessage;
    }

    private void HandleNewMessage(string message)
    {
        chatText.text += message;
    }

    [Client]
    public void Send(string message)
    {
        if(!Input.GetKeyDown(KeyCode.Return))
        {
            return;
        }
        if(string.IsNullOrWhiteSpace(message))
        {
            return;
        }


        CmdSendMessage(message);
        //Clear Field after send
        inputField.text = string.Empty;
    }

    [Command]
    private void CmdSendMessage(string message)
    {
        RpcHandleMessage($"[{connectionToClient.connectionId}]: {message}");
    }

    [ClientRpc]
    private void RpcHandleMessage(string message)
    {
        GameObject messageBubble = Instantiate(chatBubble, this.transform);
        MessageBubble msgBubble = messageBubble.GetComponent<MessageBubble>();
        msgBubble.setUp(message);
        NetworkServer.Spawn(messageBubble);
        Destroy(messageBubble, 4f);

        OnMessage?.Invoke($"\n{message}");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
