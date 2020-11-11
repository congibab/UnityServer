using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class NetworkManager : MonoBehaviour
{
    [SerializeField]
    private string _serverAddress;
    [SerializeField]
    private int _port;
    private WebSocket ws;

    public SyncPhase _nowPhase;
    public enum SyncPhase
    {
        Idling,
        Syncing
    }


    void Awake()
    {
        _nowPhase = SyncPhase.Idling;
        NetworkConnect();

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            ws.Send("Press Button A");
        }
    }

    public void NetworkConnect()
    {
        var ca = "ws://" + _serverAddress + ":" + _port.ToString();
        Debug.Log("start Connection to" + ca);
        ws = new WebSocket(ca);

        ws.OnMessage += (object sender, MessageEventArgs e) =>
        {
            Debug.Log(e.Data);
        };

        ws.OnError += (sender, e) =>
        {
            Debug.Log("Error = " + e.Message);
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.Log("Connection fail");
            _nowPhase = SyncPhase.Idling;
        };

        ws.Connect();
        Debug.Log("Connect Success");
        _nowPhase = SyncPhase.Syncing;
    }

    public void Network_disConnect()
    {
        ws.Send("disconncet form unity user");
        ws.Close();
    }
}
