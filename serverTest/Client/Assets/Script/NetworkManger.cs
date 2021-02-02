using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class NetworkManger : MonoBehaviour
{
    static public NetworkManger instance;


    [SerializeField]
    private SocketIOComponent socket;

    [SerializeField]
    private UiManger uiManger;

    [SerializeField]
    private ChatManager chatManager;

    [SerializeField]
    private GameObject gameMangerPrefab;
    private GameObject gameMangerOBJ;

    private bool is_starting;

    private void Awake()
    {
        is_starting = false;
        instance = this;
    }

    private void Start()
    {
        socket.On("open", (SocketIOEvent e) => { 
            Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data); 
        });
        socket.On("error", (SocketIOEvent e) => { 
            Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
        });
        socket.On("close", (SocketIOEvent e) => { 
            Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data); 
        });


        socket.On("UpdatePosition", OnUpdatePosition);
        socket.On("UpdateBallPosition", OnUpdataBallPosition);
        socket.On("ballPositionReset", OnballPositionReset);
        socket.On("UpdateSore", OnUpdateSore);

        socket.On("UpdateRoomList", UpdateRoomList);

        socket.On("InitPlayerid", InitPlayerid);
        socket.On("removeRoom", removeRoom);
        socket.On("UpdateChaingLog", (SocketIOEvent e) => {
            chatManager.ReceiveMsg(e);
        });

        socket.On("GameInit", GameInit);

    }


    //===========================================
    //CallBack Function
    //===========================================

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    public void OnUpdatePosition(SocketIOEvent e)
    {
        Debug.Log("OnMovement: " + e.data);

        string data = e.data.ToString();
        UserJSON user = UserJSON.CreateFromJSON(data);

        gameMangerOBJ.GetComponent<GameManger>().Players[user.id].transform.position = new Vector3(user.x, user.y, user.z);
        //var player = players[user.id];
        //player.transform.position = new Vector3(user.x, user.y, user.z);
    }

    public void OnUpdataBallPosition(SocketIOEvent e)
    {
        string data = e.data.ToString();
        BallJSON user = BallJSON.CreateFromJSON(data);
        gameMangerOBJ.GetComponent<GameManger>().Ball.GetComponent<Ball>().Dir = new Vector3(user.Dir_X, user.Dir_Y, user.Dir_Z);
    }

    public void OnballPositionReset(SocketIOEvent e)
    {
        string data = e.data.ToString();
        BallJSON user = BallJSON.CreateFromJSON(data);
        gameMangerOBJ.GetComponent<GameManger>().Ball.transform.position = new Vector3(user.x, user.y, user.z);
    }
    
    public void OnUpdateSore(SocketIOEvent e)
    {
        string data = e.data.ToString();
        GameSYS_JSON user = GameSYS_JSON.CreateFromJSON(data);
        ClientStatus.score[0] = user.score[0];
        ClientStatus.score[1] = user.score[1];
    }
    //===========================================
    //Lobby CallBack function
    //===========================================
    public void UpdateRoomList(SocketIOEvent e)
    {
        Debug.Log("Server => Init Rooms" + e.data);
        uiManger.UpdateRoom(e);
    }

    private void InitPlayerid(SocketIOEvent e)
    {
        Debug.Log("Server => Init UUID" + e.data);
        string data = e.data.ToString();
        UserJSON user = UserJSON.CreateFromJSON(data);
        ClientStatus.UUID = user.id;
    }

    private void removeRoom(SocketIOEvent e)
    {
        Debug.Log("Server => removeRoom" + e.data);
        string data = e.data.ToString();
        RoomJSON user = RoomJSON.CreateFromJSON(data);
        Destroy(uiManger.Rooms[user.name]);
    }

    //===========================================
    //===========================================

    //===========================================
    //MainGame CallBack function
    //===========================================
    private void GameInit(SocketIOEvent e)
    {
        Debug.Log("Server => Welcom to the Room of " + ClientStatus.currentingRoom);
        string data = e.data.ToString();
        Debug.Log(data);
        RoomJSON roomJSON = RoomJSON.CreateFromJSON(data);
        ClientStatus.currentUUID = roomJSON.currnetUUID;
        if (gameMangerOBJ == null)
        {
            gameMangerOBJ = Instantiate(gameMangerPrefab);
        }
    }
}

