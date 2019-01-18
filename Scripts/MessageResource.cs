using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// Store Player Message and AI Message
/// </summary>
public class MessageResource : MonoBehaviour
{
    // Create Instance
    public static MessageResource Instance { get; private set; }
    // Messages Array
    [SerializeField] private Messages[] _messages;
    // Store in dictionary player message vs Messages
    private Dictionary<string, Messages> _msgVsPlayerMsg=new Dictionary<string, Messages>();

    private void Awake()
    {
        Instance = this; 
        // Add in dictionary player message vs messages
        for (var i = 0; i < _messages.Length; i++)
        {
            // Convert original player message to lowercase
            var lowerMsg = _messages[i].PlayerMessage.ToLower();
            if (!_msgVsPlayerMsg.ContainsKey(lowerMsg))
            {
                _msgVsPlayerMsg.Add(lowerMsg, _messages[i]);
            }
        }
    }

    /// <summary>
    /// Get Message for given msg
    /// </summary>
    /// <param name="msg">Player message</param>
    /// <returns></returns>
    public Messages GetMessage(string msg)
    {
        // Convert msg to lowercase msg and
        // Check if message is contain dictionary or not
        if (_msgVsPlayerMsg.ContainsKey(msg.ToLower()))
            return _msgVsPlayerMsg[msg.ToLower()];
        // if message is not in dictionary then AI replies with default response
        return new Messages(msg, "Sorry I didn't understand");
    }

    // Store Messages
    [Serializable]
    public struct Messages
    {
        // Player Message
        [SerializeField] private string _playerMessage;
        // AI Message for matching player message
        [SerializeField] private string _aIMessage;

        public Messages(string playerMessage, string aIMessage)
        {
            _playerMessage = playerMessage;
            _aIMessage = aIMessage;
        }

        public string AIMessage
        {
            get { return _aIMessage; }
        }

        public string PlayerMessage
        {
            get { return _playerMessage; }
        }
    }    
}
