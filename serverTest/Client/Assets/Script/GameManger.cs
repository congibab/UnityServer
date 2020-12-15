using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class GameManger : MonoBehaviour
{
    [SerializeField]
    private string[] currentUUID = new string[2];

    [SerializeField]
    private GameObject PlayerPrefab;
    [SerializeField]
    public Dictionary<string, GameObject> Players = new Dictionary<string, GameObject>();
    [SerializeField]
    Transform GameCanvas;

    private Vector3[] RespawnPosint = new Vector3[2];

    void Awake()
    {
        GameCanvas = GameObject.Find("GameCanvas").GetComponent<Transform>();
    }

    void Start()
    {
        RespawnPosint[0] = new Vector3(-800, 0, 0);
        RespawnPosint[1] = new Vector3(800, 0, 0);
        StartCoroutine(GameInit());
    }

    void Update()
    {

    }


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
            var obj = Instantiate(PlayerPrefab, GameCanvas) as GameObject;
            //obj.transform.SetParent(GameCanvas.transform);

            var player = obj.GetComponent<Player>();
            player.UUID = currentUUID[i];
            
            //if (player.UUID == ClientStatus.UUID)
            //{
            //    player.is_Local = true;
            //}

            Players.Add(currentUUID[i], obj);

            
        }
    }
}
