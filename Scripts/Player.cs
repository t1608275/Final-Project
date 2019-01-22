using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Player : MonoBehaviour
{
    //reference for thridperson player controller
    [SerializeField] private ThirdPersonUserControl _control;

    private void Start()
    {
        gamemanager.Instance.OpenedChat += OnOpenedChat;
        gamemanager.Instance.ClosedChat += OnClosedChat;
    }

    //if chat open freeze player
    private void OnOpenedChat()
    {
        _control.ISFreeze = true;
    }
    
    //if chat is closed unfreeze player
    private void OnClosedChat()
    {
        _control.ISFreeze = false;
    }
}
