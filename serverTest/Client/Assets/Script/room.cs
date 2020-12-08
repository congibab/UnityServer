using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    [SerializeField]
    Text contant;

    public string Name { get; set; }
    public string UUID { get; set; }
    public int index { get; set; }
    public List<string> currnetUUID = new List<string>();
    public UiManger uiManger { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        //btn.onClick.AddListener(uiManger.join_Room);
        btn.onClick.AddListener(() => uiManger.join_Room(this));
        contant.text = Name;
    }

}
