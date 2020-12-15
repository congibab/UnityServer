using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Player : MonoBehaviour
{
    private SocketIOComponent socket;

    public string UUID;
    //public bool is_Local = false;

    private Vector3 TempPos;
    private Vector3 Dir = new Vector3(0, 0, 0);

    private Vector3[] RespawnPosint = new Vector3[2];

    void Awake()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
    }
    void Start()
    {
        if (ClientStatus.currentUUID[1] == UUID)
        {
            transform.position = new Vector3(-60, 0, 0);
        }
        else
        {
            transform.position = new Vector3(60, 0, 0);
        }
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
        //if (!is_Local) return;
        if (ClientStatus.UUID != UUID) return;

        if (Input.GetKey(KeyCode.A))
        {
            Dir = new Vector3(-1, 0, 0);
            UserJSON data = new UserJSON();
            data.RoomName = ClientStatus.currentingRoom;
            data.id = UUID;

            data.x = transform.position.x + Dir.x * Time.deltaTime;
            data.y = transform.position.y + Dir.y * Time.deltaTime;
            data.z = transform.position.z + Dir.z * Time.deltaTime;
            string user = UserJSON.CreateToJSON(data);
            socket.Emit("MovementRequest", new JSONObject(user));
        }

        if (Input.GetKey(KeyCode.D))
        {
            Dir = new Vector3(1, 0, 0);
            UserJSON data = new UserJSON();
            data.RoomName = ClientStatus.currentingRoom;
            data.id = UUID;

            data.x = transform.position.x + Dir.x * Time.deltaTime;
            data.y = transform.position.y + Dir.y * Time.deltaTime;
            data.z = transform.position.z + Dir.z * Time.deltaTime;
            string user = UserJSON.CreateToJSON(data);
            socket.Emit("MovementRequest", new JSONObject(user));
        }

        if (Input.GetKey(KeyCode.W))
        {
            Dir = new Vector3(0, 1, 0);
            UserJSON data = new UserJSON();
            data.RoomName = ClientStatus.currentingRoom;
            data.id = UUID;

            data.x = transform.position.x + Dir.x * Time.deltaTime;
            data.y = transform.position.y + Dir.y * Time.deltaTime;
            data.z = transform.position.z + Dir.z * Time.deltaTime;
            string user = UserJSON.CreateToJSON(data);
            socket.Emit("MovementRequest", new JSONObject(user));
        }

        if (Input.GetKey(KeyCode.S))
        {
            Dir = new Vector3(0, -1, 0);
            UserJSON data = new UserJSON();
            data.RoomName = ClientStatus.currentingRoom;
            data.id = UUID;

            data.x = transform.position.x + Dir.x * Time.deltaTime;
            data.y = transform.position.y + Dir.y * Time.deltaTime;
            data.z = transform.position.z + Dir.z * Time.deltaTime;
            string user = UserJSON.CreateToJSON(data);
            socket.Emit("MovementRequest", new JSONObject(user));
        }
    }
}
