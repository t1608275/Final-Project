using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class NPC : MonoBehaviour
{
	// Create event for when mouse point to AI player 
	public event Action MouseUp;
  //  public Canvas canvas;
	
	// Update is called once per frame
	private Coroutine _replyCoroutine;

	void Update () {
		
		if (Input.GetMouseButtonUp(0))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			// Check mouse point to which object
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform != null)
				{
					// if mouse point to AI then call MouseUp function 
					if (hit.transform.gameObject.layer == 9)
					{
						if (MouseUp!=null)
						{
							MouseUp.Invoke();
						}
					}
				}
			}
		}
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
	/// <param name="text" >player message</param>
	/// <param name="onFinished">after the AI reply call this function</param>
	public void ReplyMessage(string text, Action onFinished = null)
	{
		if (_replyCoroutine != null)
			StopCoroutine(_replyCoroutine);
		_replyCoroutine = StartCoroutine(ReplyMsgIEnumeration(text, onFinished));
	}

	/// <summary>
	/// Reply the AI message time delay
	/// </summary>
	/// <param name="text">player message</param>
	/// <param name="onFinished">after the AI reply call this function</param>
	/// <returns></returns>
	private IEnumerator ReplyMsgIEnumeration(string text, Action onFinished)
	{
		yield return new WaitForSeconds(0.5f);
		// ignore special characters
		var newM=Regex.Replace(text, @"[^\w\d\s]", "");
		newM = Regex.Replace(newM, @"[+$]", String.Empty);
		var replyMsg = MessageResource.Instance.GetMessage(newM).AIMessage;
		gamemanager.Instance.SendMesssage(replyMsg, false);

		if (onFinished!=null)
		{
			onFinished();
		}
	}
}
