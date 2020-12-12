using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class GameManger : MonoBehaviour
{
    [SerializeField]
    private string[] currentUUID = new string[2];

    void Start()
    {
        currentUUID[0] = ClientStatus.currentUUID[0];
        currentUUID[1] = ClientStatus.currentUUID[1];
    }

    void Update()
    {
        
    }

}
