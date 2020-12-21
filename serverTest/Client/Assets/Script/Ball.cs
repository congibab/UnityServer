using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Ball : MonoBehaviour
{
    private SocketIOComponent socket;

    [SerializeField]
    Ball ball;
    Vector3 viewPos;
    
    public Vector3 Dir;

    private void Awake()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
    }

    void Start()
    {
        Dir = new Vector3(-1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        viewPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewPos.y >= 1 && ClientStatus.UUID == ClientStatus.currentUUID[0])
        {
            Dir.y = -Dir.y;
            BallJSON data = new BallJSON();
            data.RoomName = ClientStatus.currentingRoom;
            data.x = transform.position.x;
            data.y = transform.position.y;
            data.z = transform.position.z;

            data.Dir_X = Dir.x;
            data.Dir_Y = Dir.y;
            data.Dir_Z = Dir.z;

            string user = BallJSON.CreateToJSON(data);
            socket.Emit("BallMovementRequest", new JSONObject(user));
        }

        if (viewPos.y <= 0 && ClientStatus.UUID == ClientStatus.currentUUID[0])
        {
            Dir.y = -Dir.y;
            BallJSON data = new BallJSON();
            data.RoomName = ClientStatus.currentingRoom;
            data.x = transform.position.x;
            data.y = transform.position.y;
            data.z = transform.position.z;

            data.Dir_X = Dir.x;
            data.Dir_Y = Dir.y;
            data.Dir_Z = Dir.z;

            string user = BallJSON.CreateToJSON(data);
            socket.Emit("BallMovementRequest", new JSONObject(user));
        }


        if (viewPos.x >= 1 && ClientStatus.UUID == ClientStatus.currentUUID[0])
        {
            //Dir.x = -Dir.x;
            BallJSON data = new BallJSON();
            data.RoomName = ClientStatus.currentingRoom;
            data.x = 0;
            data.y = 0;
            data.z = 0;
            data.Dir_X = Dir.x;
            data.Dir_Y = Dir.y;
            data.Dir_Z = Dir.z;

            string user = BallJSON.CreateToJSON(data);
            socket.Emit("BallPositionReset", new JSONObject(user));

            GameSYS_JSON data2 = new GameSYS_JSON();
            data2.RoomName = ClientStatus.currentingRoom;
            data2.score[0] = ClientStatus.score[0] + 1;         
            data2.score[1] = ClientStatus.score[1];         
            string Score = GameSYS_JSON.CreateToJSON(data2);
            socket.Emit("UpdateSore", new JSONObject(Score));

        }

        if (viewPos.x <= 0 && ClientStatus.UUID == ClientStatus.currentUUID[0])
        {
            //Dir.x = -Dir.x;
            BallJSON data = new BallJSON();
            data.RoomName = ClientStatus.currentingRoom;
            data.x = 0;
            data.y = 0;
            data.z = 0;
            data.Dir_X = Dir.x;
            data.Dir_Y = Dir.y;
            data.Dir_Z = Dir.z;

            string user = BallJSON.CreateToJSON(data);
            socket.Emit("BallPositionReset", new JSONObject(user));


            GameSYS_JSON data2 = new GameSYS_JSON();
            data2.RoomName = ClientStatus.currentingRoom;
            data2.score[0] = ClientStatus.score[0];
            data2.score[1] = ClientStatus.score[1] + 1;
            string Score = GameSYS_JSON.CreateToJSON(data2);
            socket.Emit("UpdateSore", new JSONObject(Score));

            //string user = BallJSON.CreateToJSON(data);
            //socket.Emit("BallPositionReset", new JSONObject(user));
            //ClientStatus.GameOver = true;
        }

        Vector3 Speed = Dir * Time.deltaTime * 5;
        transform.position += Speed;
    }

    private void FixedUpdate()
    {
     
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && ClientStatus.UUID == ClientStatus.currentUUID[0])
        {
            Dir.x = -Dir.x;
            BallJSON data = new BallJSON();
            data.RoomName = ClientStatus.currentingRoom;
            data.x = transform.position.x;
            data.y = transform.position.y;
            data.z = transform.position.z;
            
            data.Dir_X = Dir.x;
            data.Dir_Y = Dir.y;
            data.Dir_Z = Dir.z;

            string user = BallJSON.CreateToJSON(data);
            socket.Emit("BallMovementRequest", new JSONObject(user));
        }

        //if (col.gameObject.tag == "Player")
        //{
        //    Dir.x = -Dir.x;
        //}
    }   
}
