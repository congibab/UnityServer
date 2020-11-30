using System.Collections;
using UnityEngine;
using SocketIO;

public class NetworkManger : MonoBehaviour
{

    [SerializeField]
    private SocketIOComponent socket;
    [SerializeField]
    private Canvas canvas;

    public void Start()
    {
        socket.On("open", TestOpen);
        socket.On("error", TestError);
        socket.On("close", TestClose);

    }

    private IEnumerator NetworkConnect()
    {
        yield return new WaitForSeconds(0.5f);

        socket.Emit("player connect");

        canvas.gameObject.SetActive(false);
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
        Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
    }

    public void TestClose(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
    }
}

