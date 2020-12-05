using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class room : MonoBehaviour
{
    [SerializeField]
    Text contant;

    public string ID { get; set; }
    public string UUID { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        contant.text = ID;
    }


}
