using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserJSON
{
    public string RoomName;
    public string id;
    public float x;
    public float y;
    public float z;
  
    public static UserJSON CreateFromJSON(string data)
    {
        return JsonUtility.FromJson<UserJSON>(data);
    }

    public static string CreateToJSON(UserJSON data)
    {
        return JsonUtility.ToJson(data);
    }
}

[Serializable]
public class RoomJSON
{
    public string name;
    public string UUID;
    public int index;
    public string[] currnetUUID = new string[2];

    public static RoomJSON CreateFromJSON(string data)
    {
        return JsonUtility.FromJson<RoomJSON>(data);
    }

    public static string CreateToJSON(RoomJSON data)
    {
        return JsonUtility.ToJson(data);
    }
}

[Serializable]
public class BallJSON
{
    public string RoomName;
    public float x;
    public float y;
    public float z;

    public float Dir_X;
    public float Dir_Y;
    public float Dir_Z;

    public static BallJSON CreateFromJSON(string data)
    {
        return JsonUtility.FromJson<BallJSON>(data);
    }

    public static string CreateToJSON(BallJSON data)
    {
        return JsonUtility.ToJson(data);
    }
}