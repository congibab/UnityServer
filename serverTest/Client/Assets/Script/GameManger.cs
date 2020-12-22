using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class GameManger : MonoBehaviour
{
    private SocketIOComponent socket;

    [SerializeField]
    private string[] currentUUID = new string[2];

    [SerializeField]
    private GameObject PlayerPrefab;
    [SerializeField]
    private GameObject BallPrefab;

    public Dictionary<string, GameObject> Players = new Dictionary<string, GameObject>();
    public GameObject Ball;

    private Vector3[] RespawnPosint = new Vector3[2];

    private UiManger uimager;

    void Awake()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
        uimager = GameObject.Find("UiManger").GetComponent<UiManger>();

        ClientStatus.GameOver = false;
    }

    void Start()
    {
        RespawnPosint[0] = new Vector3(-9, 0, 0);
        RespawnPosint[1] = new Vector3(9, 0, 0);
        StartCoroutine(GameInit());
    }

    void Update()
    {
        if (ClientStatus.score[0] >= 5)
        {
            ClientStatus.score[0] = 0;
            ClientStatus.score[1] = 0;
            ClientStatus.GameOver = true;
        }

        else if (ClientStatus.score[1] >= 5)
        {
            ClientStatus.score[0] = 0;
            ClientStatus.score[1] = 0;
            ClientStatus.GameOver = true;
        }
    }

    void LateUpdate()
    {
        if(ClientStatus.GameOver)
        {
            RoomJSON data = new RoomJSON();
            data.name = ClientStatus.currentingRoom;
            data.UUID = ClientStatus.UUID;
            string Data = RoomJSON.CreateToJSON(data);
            socket.Emit("joinlobby", new JSONObject(Data));

            ClientStatus.currentingRoom = "lobby";
            Destroy(Players[ClientStatus.currentUUID[0]]);
            Destroy(Players[ClientStatus.currentUUID[1]]);
            Destroy(Ball);
            ClientStatus.currentUUID[0] = "";
            ClientStatus.currentUUID[1] = "";

            uimager.ReturnLobby();
            Destroy(this.gameObject);
        }
    }
#if UNITY_EDITOR
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator GameInit()
    {

        var Name = ClientStatus.UUID;

        yield return null;

        currentUUID[0] = ClientStatus.currentUUID[0];

        for (int i = 0; i < 2; i++)
        {
            var obj = Instantiate(PlayerPrefab, RespawnPosint[i], Quaternion.identity) as GameObject;

            var player = obj.GetComponent<Player>();
            player.UUID = currentUUID[i];

            Players.Add(currentUUID[i], obj);

        }
    }
#else
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator GameInit()
    {

        var Name = ClientStatus.UUID;

        yield return new WaitUntil(() => ClientStatus.currentUUID[1] != "");

        currentUUID[0] = ClientStatus.currentUUID[0];
        currentUUID[1] = ClientStatus.currentUUID[1];
         
        for (int i = 0; i < 2; i++)
        {
            var obj = Instantiate(PlayerPrefab, RespawnPosint[i], Quaternion.identity) as GameObject;

            var player = obj.GetComponent<Player>();
            player.UUID = currentUUID[i];
            
            Players.Add(currentUUID[i], obj);
        }

        Ball = Instantiate(BallPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
    }
#endif

    public Texture2D icon;

    void OnGUI()
    {
        for(int i = 0; i < ClientStatus.score[0]; i++)
        {
            GUI.Box(new Rect(10 + (50 * i), Screen.height - 60, 50, 50), icon);
        }

        for (int i = 0; i < ClientStatus.score[1]; i++)
        {
            GUI.Box(new Rect((Screen.width - 60) - (50 * i), Screen.height - 60, 50, 50), icon);
        }
        //GUI.Box(new Rect(10, Screen.height - 60, 50, 50), icon);
        //GUI.Box(new Rect(Screen.width - 60, Screen.height - 60, 50, 50), icon);
        //GUI.Box(new Rect(Screen.width/2 - 50, Screen.height - 60, 100, 50), "sa");
    }
}

