using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManger : MonoBehaviour
{
    [SerializeField]
    Canvas mainCanvas;
    [SerializeField]
    InputField inputField;
    [SerializeField]
    Transform Scroll_View_Content;
    [SerializeField]
    GameObject RoomOBJ;

    void start()
    {

    }

    public void CreateRoom()
    {
        if (inputField.text == "") return;
        var obj = Instantiate(RoomOBJ) as GameObject;
        obj.transform.SetParent(Scroll_View_Content.transform);

        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        room Room = obj.GetComponent<room>();

        rectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
        rectTransform.localScale = new Vector3(1, 1, 1);
        Room.ID = inputField.text;
    }
}