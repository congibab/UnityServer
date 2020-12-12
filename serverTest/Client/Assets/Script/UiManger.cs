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

    private Dictionary<string, GameObject> Rooms;

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
        ClientStatus.currentingRoom = inputField.text;
        data.name = inputField.text;
        data.UUID = ClientStatus.UUID;
       // data.currnetUUID[0] = ClientStatus.UUID;

        string Data = RoomJSON.CreateToJSON(data);
        socket.Emit("creatRoom", new JSONObject(Data));

        //SceneManager.LoadScene("Game");
        //DontDestroyOnLoad(socket.gameObject);
        //DontDestroyOnLoad(networkManger.gameObject);
        //DontDestroyOnLoad(this.gameObject);

        socket.Emit("joinRoom", new JSONObject(Data));
        mainCanvas.gameObject.SetActive(false);
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
        //room.currnetUUID.Add(roomJSON.UUID);

        Rooms[room.Name] = obj; 
    }

    /// <summary>
    /// 
    /// </summary>
    public void join_Room(Room target)
    {
        //SceneManager.LoadScene("Game");
        //DontDestroyOnLoad(socket.gameObject);
        //DontDestroyOnLoad(networkManger.gameObject);
        //DontDestroyOnLoad(this.gameObject);

        RoomJSON data = new RoomJSON();
        ClientStatus.currentingRoom = target.Name;
        data.name = target.Name;
        data.UUID = ClientStatus.UUID;
        data.index = target.index;


        string Data = RoomJSON.CreateToJSON(data);
        socket.Emit("joinRoom", new JSONObject(Data));
        mainCanvas.gameObject.SetActive(false);

        //Destroy(Rooms[target.name]);
    }

    void OnGUI()
    {
        string UUID = ClientStatus.currentUUID[0].ToString();
        GUI.Box(new Rect(20, 10, 250, 25), UUID);
        
        string UUID1 = ClientStatus.currentUUID[1].ToString();
        GUI.Box(new Rect(Screen.width - 250, 10, 250, 25), UUID1);
    }
}
