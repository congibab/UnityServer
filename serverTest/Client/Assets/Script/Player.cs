using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Player : MonoBehaviour
{
    private SocketIOComponent socket;

    public string UUID;

    private Vector3 Pos;
    public bool is_Local = false;

    void Awake()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
    }
    void Start()
    {

    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        playerInput();
    }

    void playerInput()
    {
        if (!is_Local) return;

        if (Input.GetKey(KeyCode.A))
        {
            UserJSON data = new UserJSON();
            data.RoomName = ClientStatus.currentingRoom;
            data.id = UUID;
            data.x = transform.position.x - Time.deltaTime;
            data.y = transform.position.y;
            data.z = transform.position.z;
            string user = UserJSON.CreateToJSON(data);
            socket.Emit("MovementRequest", new JSONObject(user));
        }

        if (Input.GetKey(KeyCode.D))
        {
            UserJSON data = new UserJSON();
            data.RoomName = ClientStatus.currentingRoom;
            data.id = UUID;
            data.x = transform.position.x + Time.deltaTime;
            data.y = transform.position.y;
            data.z = transform.position.z;
            string user = UserJSON.CreateToJSON(data);
            socket.Emit("MovementRequest", new JSONObject(user));
        }

        if (Input.GetKey(KeyCode.W))
        {
            UserJSON data = new UserJSON();
            data.RoomName = ClientStatus.currentingRoom;
            data.id = UUID;
            data.x = transform.position.x;
            data.y = transform.position.y + Time.deltaTime;
            data.z = transform.position.z;
            string user = UserJSON.CreateToJSON(data);
            socket.Emit("MovementRequest", new JSONObject(user));
        }

        if (Input.GetKey(KeyCode.S))
        {
            UserJSON data = new UserJSON();
            data.RoomName = ClientStatus.currentingRoom;
            data.id = UUID;
            data.x = transform.position.x;
            data.y = transform.position.y - Time.deltaTime;
            data.z = transform.position.z;
            string user = UserJSON.CreateToJSON(data);
            socket.Emit("MovementRequest", new JSONObject(user));
        }
    }
}
