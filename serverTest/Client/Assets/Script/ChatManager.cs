using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class ChatManager : MonoBehaviour
{
    [SerializeField]
    List<string> chatList = new List<string>();
    [SerializeField] Button sendBtn;
    [SerializeField] Text chatLog;
    [SerializeField] Text chattingList;
    [SerializeField] InputField input;
    [SerializeField] ScrollRect scroll_rect;
    string chatters;

    SocketIOComponent socket;

    private void Awake()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SendButtonOnClicked()
    {
        if (input.text.Equals("")) { Debug.Log("Empty"); return; }

        input.ActivateInputField();

        JSONObject msg = new JSONObject(JSONObject.Type.STRING);
        msg.AddField("User", "");
        msg.AddField("Message", input.text);


        socket.Emit("UpdateChaingLog", msg);
        input.text = "";
    }

    //public void ReceiveMsg(string msg)
    public void ReceiveMsg(SocketIOEvent e)
    {
        //JSONObject json = new JSONObject(e.data);
        string user = e.data.GetField("User").str;
        string msg = e.data.GetField("Message").str;
        chatLog.text += "\n" + user + " : " + msg;
        scroll_rect.verticalNormalizedPosition = 0.0f;
    }
}
