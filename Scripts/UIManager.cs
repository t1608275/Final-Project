using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public event Action<ChatBotType> BotBtnClicked;

    // get reference of bot slection pannel
    [SerializeField] private GameObject _botSelectionPannel;

    // get simple button reference
    [SerializeField] private Button _simpleBtn;

    // get advance button reference
    [SerializeField] private Button _advanceBtn;

    public GameObject BotSelectionPannel => _botSelectionPannel;

    private void Awake()
    {
        Instance = this;
        // Set listner for simple bot
        _simpleBtn.onClick.AddListener(() => { BotTypeBtn(ChatBotType.Simple); });
        // Set listner for advance bot
        _advanceBtn.onClick.AddListener(() => { BotTypeBtn(ChatBotType.Advance); });
    }

    private void BotTypeBtn(ChatBotType type)
    {
        BotBtnClicked?.Invoke(type);
    }
}
