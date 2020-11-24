using System;
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

    private ObjectStatus objectStatus;
    public GameObject testObject;

    private string jsonData;

    public SyncPhase _nowPhase = SyncPhase.Idling;
    public enum SyncPhase
    {
        Idling,
        Syncing
    }


    void Awake()
    {
        objectStatus = new ObjectStatus();
        _nowPhase = SyncPhase.Idling;
        NetworkConnect();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) && _nowPhase == SyncPhase.Syncing)
        {
            objectStatus.Types = "Player1_input";
          
            objectStatus.player1.Pos_x = testObject.transform.position.x;
            objectStatus.player1.Pos_y = testObject.transform.position.y;
            objectStatus.player1.Pos_z = testObject.transform.position.z;
            var json = JsonUtility.ToJson(objectStatus);
            ws.Send(json);
        }
    }

    public void NetworkConnect()
    {
        if (_nowPhase == SyncPhase.Syncing) return;
        var ca = "ws://" + _serverAddress + ":" + _port.ToString();
        Debug.Log("start Connection to" + ca);
        ws = new WebSocket(ca);

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Connect Success");
            _nowPhase = SyncPhase.Syncing;
        };

        ws.OnMessage += (object sender, MessageEventArgs e) =>
        {
            Debug.Log("==>>" + e.Data);
            try
            {
                var test = JsonUtility.FromJson<ObjectStatus>(e.Data);
                Debug.Log(test.Types);

                switch (test.Types)
                {
                    case "init":
                        Debug.Log(test.UUID);
                        break;
                    case "input":
                        
                        break;
                    default:
                        break;
                }
                objectStatus = test;

            }
            catch(Exception ex)
            {
                Debug.Log("not json");
            }

        };

        ws.OnError += (sender, e) =>
        {
            Debug.Log("Error = " + e.Message);
            _nowPhase = SyncPhase.Idling;
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.Log("Connection fail");
            _nowPhase = SyncPhase.Idling;
        };
        ws.Connect();
    }

    public void Network_disConnect()
    {
        if (_nowPhase == SyncPhase.Idling) return;
        ws.Send("disconncet form unity user");
        ws.Close();
    }
}
