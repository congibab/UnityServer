using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SocketIO;


public class UiManger : MonoBehaviour
{
    List<GameObject> RoomList = new List<GameObject>();

    private SocketIOComponent socket;
    private NetworkManger networkManger;

    [SerializeField]
    private Canvas mainCanvas;
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private Transform Scroll_View_Content;
    [SerializeField]
    private GameObject RoomOBJ;

    void Awake()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
        networkManger = GameObject.Find("NetworkManger").GetComponent<NetworkManger>();
    }

    void Start()
    {
        
    }

    public void CreateRoom()
    {
        if (inputField.text == "") return;

        RoomJSON data = new RoomJSON();
        data.name = inputField.text;
        data.UUID = ClientStatus.UUID;
        data.currnetUUID[0] = ClientStatus.UUID;

        string Data = RoomJSON.CreateToJSON(data);
        socket.Emit("creatRoom", new JSONObject(Data));

        SceneManager.LoadScene("Game");
        DontDestroyOnLoad(socket.gameObject);
        DontDestroyOnLoad(networkManger.gameObject);
        DontDestroyOnLoad(this.gameObject);

        socket.Emit("joinRoom", new JSONObject(Data));


    }

    public void UpdateRoom(SocketIOEvent e)
    {
        string data = e.data.ToString();
        RoomJSON roomJSON = RoomJSON.CreateFromJSON(data);

        var obj = Instantiate(RoomOBJ) as GameObject;
        obj.transform.SetParent(Scroll_View_Content.transform);

        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        Room room = obj.GetComponent<Room>();

        rectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
        rectTransform.localScale = new Vector3(1, 1, 1);
        room.uiManger = this;
        room.Name = roomJSON.name;
        room.index = roomJSON.index;
        room.UUID = roomJSON.UUID;
        
        room.currnetUUID.Add(roomJSON.UUID);

    }

    /// <summary>
    /// 
    /// </summary>
    public void join_Room(Room target)
    {
        SceneManager.LoadScene("Game");
        DontDestroyOnLoad(socket.gameObject);
        DontDestroyOnLoad(networkManger.gameObject);
        DontDestroyOnLoad(this.gameObject);


        RoomJSON data = new RoomJSON();
        ClientStatus.currentingRoom = target.Name;
        data.name = target.Name;
        data.UUID = ClientStatus.UUID;
        data.index = target.index;

        data.currnetUUID[1] = ClientStatus.UUID;

        string Data = RoomJSON.CreateToJSON(data);
        socket.Emit("joinRoom", new JSONObject(Data));
    }
}