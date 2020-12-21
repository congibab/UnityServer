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
    //[SerializeField]
    //private GameObject SocketIOPrefab;
    //GameObject test;

    [SerializeField]
    private UiManger uiManger;

    [SerializeField]
    private GameObject gameMangerPrefab;
    private GameObject gameMangerOBJ;

    //=================================
    //C++ = map class
    Dictionary<string, GameObject> players;
    //List<string> UUID_list = new List<string>();
    List<GameObject> RoomList = new List<GameObject>();
    //=================================

    private bool is_starting;

    private void Awake()
    {
        is_starting = false;
        instance = this;
    }

    private void Start()
    {
        socket.On("open", TestOpen);
        socket.On("error", TestError);
        socket.On("close", TestClose);

        //socket.On("OtherSpawn", OnOtherSpawn);
        //socket.On("PlayableSpawn", OnplayableSpawn);

        socket.On("UpdatePosition", OnUpdatePosition);
        socket.On("UpdateBallPosition", OnUpdataBallPosition);
        socket.On("ballPositionReset", OnballPositionReset);
        socket.On("UpdateSore", OnUpdateSore);

        socket.On("UpdateRoomList", UpdateRoomList);
        socket.On("InitPlayerid", InitPlayerid);

        
        socket.On("GameInit", GameInit);


        players = new Dictionary<string, GameObject>();
        //Dictionary<string, int> test = new Dictionary<string, int>();
        //test.Add("test1", 1);
        //test.Add("test2", 2);
        //test.Add("test3", 3);
        //test.Add("test4", 4);
        //foreach (KeyValuePair<string, int> pair in test)
        //{
        //    Debug.Log(pair.Key + " " + pair.Value);
        //}

        //test.Clear();
        //test.Add("abcd", 12);
        //foreach (KeyValuePair<string, int> pair in test)
        //{
        //    Debug.Log(pair.Key + " " + pair.Value);
        //}
    }


    private IEnumerator NetworkConnect()
    {
        yield return new WaitForSeconds(0.5f);
        socket.Emit("NetworkStart");
        yield return new WaitForSeconds(1.0f);

    }

    //===========================================
    //UI Button Function
    //===========================================
    public void GameStartButton()
    {
        is_starting = true;
        StartCoroutine(NetworkConnect());
    }
    //===========================================
    //CallBack Function
    //===========================================
    public void TestOpen(SocketIOEvent e)
    {
        is_starting = false;
        Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
    }

    public void TestError(SocketIOEvent e)
    {
        is_starting = false;
        Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
    }

    public void TestClose(SocketIOEvent e)
    {
        is_starting = false;
        Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
    }

    //====================================================
    //====================================================


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
    //===========================================
    //===========================================

    //private string IPAdress = "localhost:3000";

    //void OnGUI()
    //{
    //    IPAdress = GUI.TextField(new Rect(20, Screen.height / 2, 100, 30), IPAdress);

    //    if (GUI.RepeatButton(new Rect(20, Screen.height / 2 + 30, 100, 30), "Connect"))
    //    {
    //        test = Instantiate(SocketIOPrefab);
    //        test.GetComponent<SocketIOComponent>().url = "ws://" + IPAdress + "/socket.io/?EIO=4&transport=websocket";
    //    }
    //}
}

