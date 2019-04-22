using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum ChatBotType
{
    Simple,
    Advance
}

public class gamemanager : MonoBehaviour
{

    public static gamemanager Instance;
    public event Action OpenedChat;
    public event Action ClosedChat;
    public int MaxMessages = 20;
    public ChatBotType BotType { get; set; }

    public GameObject chatpanel, textObject;

    public InputField chatbox;

    // Get reference for NPC
    [SerializeField] private NPC _npc;

    // Get reference for my chat box parent panel
    [SerializeField] private GameObject chatCanvas;
    [SerializeField] private float delayReplyTime = 0.3f;
    [SerializeField] private PandoraBotSystem botSystem;

    public PandoraBotSystem BotSystem => botSystem;

    private List<Message> messageList = new List<Message>();
    private Coroutine replyMsgCor;
    private bool isAlreadyChatOpen;

    private void OnValidate()
    {
        if (delayReplyTime < 0.0f)
        {
            delayReplyTime = 0;
        }
    }

    private void Awake()
    {
        Instance = this;
        // Add function NpcOnMouseUp to NPC MouseUP event
        _npc.MouseUp += NpcOnMouseUp;
        chatCanvas.gameObject.SetActive(false);
    }

    private void Start()
    {
        UIManager.Instance.BotSelectionPannel.SetActive(false);
        UIManager.Instance.BotBtnClicked += OnBotBtnClicked;
    }

    private void OnBotBtnClicked(ChatBotType obj)
    {
        BotType = obj;
        UIManager.Instance.BotSelectionPannel.SetActive(false);
        Debug.Log("Bot Click "+UIManager.Instance.BotSelectionPannel.activeSelf);
        // check my chat box parent active or not
        if (!chatCanvas.activeSelf)
        {
            chatCanvas.SetActive(true);
        }
       
    }

    private void NpcOnMouseUp()
    {
        // When click npc player open bot selection pannel
        if(isAlreadyChatOpen)
            return;
        isAlreadyChatOpen = true;
        UIManager.Instance.BotSelectionPannel.SetActive(true);
        if (OpenedChat != null) OpenedChat.Invoke();
    }

    // Update is called once per frame
    private void Update()
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
                if (replyMsgCor != null)
                {
                    StopCoroutine(replyMsgCor);
                }

                replyMsgCor = StartCoroutine(AIMsgReplyCor());
            }
        }
        else
        {
            if (!chatbox.isFocused && Input.GetKeyDown(KeyCode.Return))

                chatbox.ActivateInputField();
        }

        if (chatCanvas && Input.GetKeyDown(KeyCode.Escape))
        {
            OnChatClose();
        }
        
    }

    private IEnumerator AIMsgReplyCor()
    {
        yield return new WaitForSeconds(delayReplyTime);
        var pendObj = AIThinkingPending();
        // AI perform the reply message
        // After the AI reply message then enable input field interactable
        _npc.ReplyMessage(chatbox.text, ((isSucces, result) =>
        {
            DestroyImmediate(pendObj);
            var reply = isSucces ? result : "Error";
            SendMesssage(reply, false);
            chatbox.interactable = true;
            chatbox.Select();
        }));
        chatbox.text = "";
    }


    private void OnChatClose()
    {
        chatCanvas.SetActive(false);
        UIManager.Instance.BotSelectionPannel.SetActive(false);
        if (ClosedChat != null) ClosedChat.Invoke();
        foreach (var t in messageList)
        {
            if (t.textObject)
            {
                Destroy(t.textObject.gameObject);
            }
        }

        isAlreadyChatOpen = false;
        messageList.Clear();
    }

    private GameObject AIThinkingPending()
    {
        var newMessage = new Message {text = "typing..."};

        var newText = Instantiate(textObject, chatpanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();
        // if message is player message then arrange the text alignment in left side
        // else message is AI message then arrange the text alignment in right side
        var txtColor = newMessage.textObject.color;
        txtColor.a /= 2;
        newMessage.textObject.color = txtColor;
        newMessage.textObject.alignment = TextAnchor.MiddleRight;
        newMessage.textObject.fontStyle = FontStyle.Italic;
        newMessage.textObject.text = newMessage.text;
        return newText;
    }

    public void SendMesssage(string text, bool isLeftDir = true)
    {
        if (messageList.Count >= MaxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        var newMessage = new Message {text = text};

        var newText = Instantiate(textObject, chatpanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();
        // if message is player message then arrange the text alignment in left side
        // else message is AI message then arrange the text alignment in right side
        newMessage.textObject.alignment = isLeftDir ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight;
        newMessage.textObject.text = newMessage.text;
        messageList.Add(newMessage);
    }

    [Serializable]
    public class Message
    {
        public string text;
        public Text textObject;
    }
}