using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class test : MonoBehaviour
{
    [SerializeField]
    SocketIOComponent socket;
    // Start is called before the first frame update
    void Start()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
