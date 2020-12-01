using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkManger : MonoBehaviour
{

    [SerializeField]
    private SocketIOComponent socket;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject player;

    //C++ = map class
    Dictionary<string, GameObject> players;

    public void Start()
    {
        socket.On("open", TestOpen);
        socket.On("error", TestError);
        socket.On("close", TestClose);

        socket.On("disconnect", TestDisconnect);
        socket.On("disconnected", OnDisconnected);

        socket.On("OtherSpawn", OnOtherSpawn);
        socket.On("PlayableSpawn", OnplayableSpawn);
        
        socket.On("requestPosition", OnRequestPosition);


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
    //===========================================

    public void GameStart()
    {
        StartCoroutine(NetworkConnect());
    }

    public void TestOpen(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
    }

    public void TestError(SocketIOEvent e)
    {
        canvas.gameObject.SetActive(true);
        Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
    }

    public void TestClose(SocketIOEvent e)
    {
        canvas.gameObject.SetActive(true);
        Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
    }

    public void TestDisconnect(SocketIOEvent e)
    {
        Debug.Log("Client disconnected: " + e.data);
        canvas.gameObject.SetActive(true);

       // var player = players
        
    }

    /// <summary>
    /// none Playable object
    /// </summary>
    /// <param name="e"></param>
    public void OnOtherSpawn(SocketIOEvent e)
    {
        Debug.Log("Spawn spawned" + e.data);
        string data = e.data.ToString();
        UserJSON user = UserJSON.CreateFromJSON(data);

        GameObject p = Instantiate(player, Vector3.zero, Quaternion.identity) as GameObject;
        
        players.Add(user.id, p);
        Debug.Log("player count: " + players.Count + "// playerID: " + user.id);

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
}

