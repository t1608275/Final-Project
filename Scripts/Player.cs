using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Player : MonoBehaviour
{
    [SerializeField] private ThirdPersonUserControl _control;


    private void Start()
    {
        gamemanager.Instance.OpenedChat+=OnOpenedChat;
        gamemanager.Instance.ClosedChat+=OnClosedChat;
    }

    private void OnClosedChat()
    {
        _control.ISFreeze = false;
    }

    private void OnOpenedChat()
    {
        _control.ISFreeze = true;
    }
}
