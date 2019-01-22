using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;




public class gamemanager : MonoBehaviour
{

    public static gamemanager Instance;
    public int MaxMessages = 20;
    public event Action OpenedChat;
    public event Action ClosedChat;

    public GameObject chatpanel, textObject;
    
    public InputField chatbox;
    // Get reference for NPC
    [SerializeField] private NPC _npc;
    // Get reference for my chat box parent panel
    [SerializeField] private GameObject chatCanvas;
    [SerializeField] List<Message> messageList = new List<Message>();


    private void Awake()
    {
        Instance = this;
        // Add function NpcOnMouseUp to NPC MouseUP event
        _npc.MouseUp+=NpcOnMouseUp;
        chatCanvas.gameObject.SetActive(false);
    }

    private void NpcOnMouseUp()
    {
        // check my chat box parent active or not
        if (!chatCanvas.gameObject.activeSelf)
        {
            chatCanvas.gameObject.SetActive(true);
            if(OpenedChat != null) OpenedChat.Invoke();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (chatbox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMesssage(chatbox.text);
                // if not enabled then enabled NPC
                if (_npc.enabled == false)
                    _npc.enabled = true;
                // Block the next interact in input field until AI reply message
                chatbox.interactable = false;
                
                // AI perform the reply message
                // After the AI reply message then enable input field interactable
                _npc.ReplyMessage(chatbox.text,(() => { chatbox.interactable = true; }));
                chatbox.text = "";
            }
        }

        else
        {
            
            if (!chatbox.isFocused && Input.GetKeyDown(KeyCode.Return))

                chatbox.ActivateInputField();
        }

        //exit the chatbox by pressing escape key 

        if (chatCanvas == true && Input.GetKeyDown(KeyCode.Escape))

           {
             OnChatClose();
           }
            
            // chatCanvas.gameObject.SetActive(false);
        
        //  if (!chatbox.isFocused);

    }
    
    //turn off chatbox and clear messages
    private void OnChatClose()
    {
        chatCanvas.gameObject.SetActive(false);
        if (ClosedChat != null) ClosedChat.Invoke();
        foreach (var t in messageList)
        {
            if (t.textObject)
            {
                Destroy(t.textObject.gameObject);
            }
        }

        messageList.Clear();
    }
    
    

    public void SendMesssage(string text, bool isLeftDir = true)
    {
        if (messageList.Count >= MaxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();

        newMessage.text = text;
        // Arrange the previous message text box
        for (var i = messageList.Count - 1; i >= 0; i--)
        {
            var pos =(RectTransform) messageList[i].textObject.transform;
            var posLocalPosition = pos.localPosition;
            posLocalPosition.y += pos.sizeDelta.y;
            pos.localPosition = posLocalPosition;
        }

        GameObject newText = Instantiate(textObject, chatpanel.transform);
        
        newMessage.textObject = newText.GetComponent<Text>();
        // if message is player message then arrange the text alignment to left side
        // else message is AI message then arrange the text alignment to right side
        newMessage.textObject.alignment = isLeftDir ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight;
        newMessage.textObject.text = newMessage.text;
        messageList.Add(newMessage);
    }

    [System.Serializable]
    public class Message
    {
        public string text;
        public Text textObject;
    }
}
