using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkManger : MonoBehaviour
{
    static public NetworkManger instance;

    [SerializeField]
    private SocketIOComponent socket;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject player;

    //C++ = map class
    Dictionary<string, GameObject> players;
    List<string> UUID_list = new List<string>();

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

        socket.On("disconnect", TestDisconnect);
        socket.On("disconnected", OnDisconnected);

        socket.On("OtherSpawn", OnOtherSpawn);
        socket.On("PlayableSpawn", OnplayableSpawn);

        socket.On("requestPosition", OnRequestPosition);
        socket.On("UpdatePosition", OnUpdatePosition);


        players = new Dictionary<string, GameObject>();
    }

    private IEnumerator NetworkConnect()
    {
        canvas.gameObject.SetActive(false);
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
        canvas.gameObject.SetActive(true);
        Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
    }

    public void TestError(SocketIOEvent e)
    {
        is_starting = false;
        canvas.gameObject.SetActive(true);
        Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
    }

    public void TestClose(SocketIOEvent e)
    {
        is_starting = false;
        canvas.gameObject.SetActive(true);
        Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
    }

    public void TestDisconnect(SocketIOEvent e)
    {
        Debug.Log("Server disconnected: " + e.data);
        is_starting = false;
        for (int i = 0; i < UUID_list.Count; i++)
        {
            var player = players[UUID_list[i]];
            Destroy(player);
            players.Remove(UUID_list[i]);
            UUID_list.RemoveAt(i);
        }
        canvas.gameObject.SetActive(true);
    }

    //====================================================
    //====================================================

    /// <summary>
    /// none Playable object
    /// </summary>
    /// <param name="e"></param>
    public void OnOtherSpawn(SocketIOEvent e)
    {
        if (!is_starting) return;

        Debug.Log("Spawn spawned" + e.data);
        string data = e.data.ToString();
        UserJSON user = UserJSON.CreateFromJSON(data);

        GameObject p = Instantiate(player, Vector3.zero, Quaternion.identity) as GameObject;
        players.Add(user.id, p);
        Debug.Log("player count: " + players.Count + "// playerID: " + user.id);

        UUID_list.Add(user.id);

        p.GetComponent<Player>().UUID = user.id;
        p.GetComponent<Player>().is_Local = false;
    }

    /// <summary>
    /// Playable object
    /// </summary>
    /// <param name="e"></param>
    public void OnplayableSpawn(SocketIOEvent e)
    {
        Debug.Log("Playable spawned" + e.data);
        string data = e.data.ToString();
        UserJSON user = UserJSON.CreateFromJSON(data);

        GameObject p = Instantiate(player, Vector3.zero, Quaternion.identity) as GameObject;

        players.Add(user.id, p);
        Debug.Log("player count: " + players.Count + "// playerID: " + user.id);

        UUID_list.Add(user.id);

        p.GetComponent<Player>().UUID = user.id;
        p.GetComponent<Player>().is_Local = true;
    }

    public void OnDisconnected(SocketIOEvent e)
    {
        Debug.Log("Client disconnected: " + e.data);

        string data = e.data.ToString();
        UserJSON user = UserJSON.CreateFromJSON(data);

        var player = players[user.id];
        Destroy(player);
        players.Remove(user.id);
    }

    public void OnRequestPosition(SocketIOEvent e)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    public void OnUpdatePosition(SocketIOEvent e)
    {
        Debug.Log("OnMovement: " + e.data);

        string data = e.data.ToString();
        UserJSON user = UserJSON.CreateFromJSON(data);

        var player = players[user.id];
        player.transform.position = new Vector3(user.x, user.y, user.z);
    }
    //===========================================
    //===========================================
}

