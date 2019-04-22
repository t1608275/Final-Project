using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
	// Create event for when mouse point to AI player 
	public event Action MouseUp;
  //  public Canvas canvas;
	
	// Update is called once per frame
	private Coroutine _replyCoroutine;
	private Camera _camera;
    public int LayerMask { get; set; }

    public Camera Camera
    {
        get
        {
           return _camera;
        }
        set { _camera = value; }
    }

	private void Awake()
	{
		_camera=Camera.main;
        LayerMask = 9;
    }

    void Update()
    {

        if (Input.GetMouseButtonUp(0))
        {
            if (CheckInteract(Input.mousePosition, LayerMask))
            {
                if (MouseUp != null)
                {
                    MouseUp.Invoke();
                }
            }
        }
    }

    public bool CheckInteract(Vector3 pos, int layer)
    {
        var ray = _camera.ScreenPointToRay(pos);
        RaycastHit hit;
        // Check mouse point to which object
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform != null)
            {
                // if mouse point to AI then call MouseUp function 
                if (hit.transform.gameObject.layer == layer)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void OnMouseEnter()
    {
        Debug.Log("Hello");
    }

    public void OnMouseExit()
    {
        Debug.Log("Bye");   
    }

    public void OnMouseUp()
    {
      //  canvas.enabled = true;
    }

    /// <summary>
    /// Reply the AI message for player message
    /// </summary>
    /// <param name="text">player message</param>
    /// <param name="onFinished">after the AI reply call this function</param>
    public void ReplyMessage(string text, Action<bool, string> onFinished = null)
    {
        var botSystem = gamemanager.Instance.BotSystem;
        // if ChatBot type Advance then open bot system message system else open simple message system
        if (gamemanager.Instance.BotType == ChatBotType.Advance)
            botSystem.SentMessage(text, onFinished);
        else
        {
            MessageResource.Instance.GetMessage(text, onFinished);
        }
    }

}
