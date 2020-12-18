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
    private GameObject BallPrefab;

    public Dictionary<string, GameObject> Players = new Dictionary<string, GameObject>();
    public GameObject Ball;

    private Vector3[] RespawnPosint = new Vector3[2];

    void Awake()
    {
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
            var obj = Instantiate(PlayerPrefab, RespawnPosint[i], Quaternion.identity) as GameObject;

            var player = obj.GetComponent<Player>();
            player.UUID = currentUUID[i];
            
            Players.Add(currentUUID[i], obj);
        }

        Ball = Instantiate(BallPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
    }
}
