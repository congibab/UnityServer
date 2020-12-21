using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class test : MonoBehaviour
{
    public string T;
    GameObject a;


    test(string url)
    {
        T = url;
    }
    [SerializeField]
    SocketIOComponent socket;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
