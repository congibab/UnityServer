﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Player : MonoBehaviour
{
    private SocketIOComponent socket;

    public string UUID;

    private Vector3 Dir = new Vector3(0, 0, 0);
    [SerializeField]
    private float MoveSpeed = 10.0f;
    [SerializeField]
    private Material[] material;

    Vector3 viewPos;
    Vector3 viewPos2;

    NetworkManger network;

    void Awake()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
    }
    void Start()
    {
        Renderer rend = GetComponent<Renderer>();

        //for(int i = 0; i < ClientStatus.currentUUID.Length; i++)
        //{
        //    if (ClientStatus.currentUUID[i] == UUID)
        //    {
        //        rend.material = material[i];
        //        break;
        //    }
        //}

        if (ClientStatus.UUID == UUID)
        {
            rend.material = material[0];
        }

        else
        {
            rend.material = material[1];
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
        viewPos = Camera.main.WorldToViewportPoint(transform.position + transform.localScale / 2);
        viewPos2 = Camera.main.WorldToViewportPoint(transform.position - transform.localScale / 2);

        //if (!is_Local) return;
        if (ClientStatus.UUID != UUID) return;

        //if (Input.GetKey(KeyCode.A))
        //{
        //    Dir = new Vector3(-1, 0, 0);
        //    UserJSON data = new UserJSON();
        //    data.RoomName = ClientStatus.currentingRoom;
        //    data.id = UUID;

        //    data.x = transform.position.x + Dir.x * Time.deltaTime;
        //    data.y = transform.position.y + Dir.y * Time.deltaTime;
        //    data.z = transform.position.z + Dir.z * Time.deltaTime;
        //    string user = UserJSON.CreateToJSON(data);
        //    socket.Emit("MovementRequest", new JSONObject(user));
        //}

        //if (Input.GetKey(KeyCode.D))
        //{
        //    Dir = new Vector3(1, 0, 0);
        //    UserJSON data = new UserJSON();
        //    data.RoomName = ClientStatus.currentingRoom;
        //    data.id = UUID;

        //    data.x = transform.position.x + Dir.x * Time.deltaTime;
        //    data.y = transform.position.y + Dir.y * Time.deltaTime;
        //    data.z = transform.position.z + Dir.z * Time.deltaTime;
        //    string user = UserJSON.CreateToJSON(data);
        //    socket.Emit("MovementRequest", new JSONObject(user));
        //}
        if (Input.GetKey(KeyCode.W) && viewPos.y <= 1.0f)
        {
            Dir = new Vector3(0, 1, 0);
            UserJSON data = new UserJSON();
            data.RoomName = ClientStatus.currentingRoom;
            data.id = UUID;

            data.x = transform.position.x + Dir.x * Time.deltaTime * MoveSpeed;
            data.y = transform.position.y + Dir.y * Time.deltaTime * MoveSpeed;
            data.z = transform.position.z + Dir.z * Time.deltaTime * MoveSpeed;
            string user = UserJSON.CreateToJSON(data);
            socket.Emit("MovementRequest", new JSONObject(user));
        }

        if (Input.GetKey(KeyCode.S) && viewPos2.y >= 0.0f)
        {
            Dir = new Vector3(0, -1, 0);
            UserJSON data = new UserJSON();
            data.RoomName = ClientStatus.currentingRoom;
            data.id = UUID;

            data.x = transform.position.x + Dir.x * Time.deltaTime * MoveSpeed;
            data.y = transform.position.y + Dir.y * Time.deltaTime * MoveSpeed;
            data.z = transform.position.z + Dir.z * Time.deltaTime * MoveSpeed;
            string user = UserJSON.CreateToJSON(data);
            socket.Emit("MovementRequest", new JSONObject(user));
        }
    }
}
