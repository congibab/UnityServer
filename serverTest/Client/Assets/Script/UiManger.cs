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
    private GameObject test;
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
        //var obj = Instantiate(RoomOBJ) as GameObject;
        //obj.transform.SetParent(Scroll_View_Content.transform);

        //RectTransform rectTransform = obj.GetComponent<RectTransform>();
        //Room room = obj.GetComponent<Room>();

        //rectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
        //rectTransform.localScale = new Vector3(1, 1, 1);
        //room.ID = inputField.text;
        //room.uiManger = this;

        //RoomList.Add(obj);

        RoomJSON data = new RoomJSON();
        data.name = inputField.text;
//        data.UUID = 

        string Data = RoomJSON.CreateToJSON(data);
        socket.Emit("creatRoom", new JSONObject(Data));
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
        room.Name = roomJSON.name;
        room.uiManger = this;
    }

    public void join_Room()
    {
        SceneManager.LoadScene("Game");
        DontDestroyOnLoad(socket.gameObject);
        DontDestroyOnLoad(networkManger.gameObject);
    }
}