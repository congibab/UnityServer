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

    void Start()
    {
        //GameInit();
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
            var obj = Instantiate(PlayerPrefab);
            var player = obj.GetComponent<Player>();
            player.UUID = currentUUID[i];
            
            if (player.UUID == ClientStatus.UUID)
            {
                player.is_Local = true;
            }

            Players.Add(currentUUID[i], obj);

            
        }
    }
}
