using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Player : MonoBehaviour
{
    private SocketIOComponent socket;

    public string UUID;

    private Vector3 Pos;
    public bool is_Local;

    void Awake()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();    
    }
    void Start()
    {

    }

    void Update()
    {
        playerInput();
    }

    void playerInput()
    {
        if (Input.GetKey(KeyCode.A) && is_Local)
        {
            UserJSON data = new UserJSON();
            data.id = UUID;
            data.x = transform.position.x - Time.deltaTime;
            data.y = transform.position.y;
            data.z = transform.position.z;
            string user = UserJSON.CreateToJSON(data);
            socket.Emit("Movement", new JSONObject(user));
        }

        if (Input.GetKey(KeyCode.D) && is_Local)
        {
            UserJSON data = new UserJSON();
            data.id = UUID;
            data.x = transform.position.x + Time.deltaTime;
            data.y = transform.position.y;
            data.z = transform.position.z;
            string user = UserJSON.CreateToJSON(data);
            socket.Emit("Movement", new JSONObject(user));
        }

        if (Input.GetKey(KeyCode.W) && is_Local)
        {
            UserJSON data = new UserJSON();
            data.id = UUID;
            data.x = transform.position.x;
            data.y = transform.position.y + Time.deltaTime;
            data.z = transform.position.z;
            string user = UserJSON.CreateToJSON(data);
            socket.Emit("Movement", new JSONObject(user));
        }

        if (Input.GetKey(KeyCode.S) && is_Local)
        {
            UserJSON data = new UserJSON();
            data.id = UUID;
            data.x = transform.position.x;
            data.y = transform.position.y - Time.deltaTime;
            data.z = transform.position.z;
            string user = UserJSON.CreateToJSON(data);
            socket.Emit("Movement", new JSONObject(user));
        }
    }
}
