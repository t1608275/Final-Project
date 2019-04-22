using System;
using System.Collections;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class PandoraBotSystem : MonoBehaviour
{
    [SerializeField] private string botId = "";
    private Coroutine _sentMsgCor;
    private string lastBotMsg = "";

    public void SentMessage(string ques, Action<bool, string> onFinished = null)
    {
        if (_sentMsgCor != null)
            StopCoroutine(_sentMsgCor);
        _sentMsgCor = StartCoroutine(SentMessageCor(ques, onFinished));
    }

    private IEnumerator SentMessageCor(string ques, Action<bool, string> onFinished = null)
    {
        var orginTxt = ques.Replace(" ", "%20").Replace("?", "%3F");
        lastBotMsg = lastBotMsg.Replace(" ", "%20").Replace("?", "%3F");
        var uri = "https://api.pandorabots.com/atalk?botkey=" + botId + "&input=" + orginTxt + "&that=" + lastBotMsg;
        var www = UnityWebRequest.Post(
            uri, ""
        );
        yield return www.SendWebRequest();
        var isSucces = false;
        var result = "";

        if (!www.isNetworkError && !www.isHttpError)
        {
            var responseMsg = JsonUtility.FromJson<ResponseMsg>(www.downloadHandler.text);

            if (responseMsg.Response.Length > 0)
            {
                isSucces = true;
                var msg = responseMsg.Response.FirstOrDefault();
                try
                {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(msg);
                    var wrappNode = xmlDocument.LastChild;
                    if (wrappNode.ChildNodes.Count > 0)
                        result = wrappNode.ChildNodes.Item(Random.Range(0, wrappNode.ChildNodes.Count)).InnerText;
                    else
                    {
                        result = "Something went wrong!";
                    }
                }
                catch (XmlException e)
                {
                    // Debug.Log("Error found: " + e.Message);
                    result = msg;
                }
            }
        }

        lastBotMsg = result;
        onFinished?.Invoke(isSucces, result);
    }

    [Serializable]
    public struct ResponseMsg
    {
        [SerializeField] private string status;
        [SerializeField] private string[] responses;

        public string Status => status;
        public string[] Response => responses;
    }
}
