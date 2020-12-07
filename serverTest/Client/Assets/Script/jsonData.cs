using System;
using UnityEngine;

[Serializable]
public class UserJSON
{
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

    public static RoomJSON CreateFromJSON(string data)
    {
        return JsonUtility.FromJson<RoomJSON>(data);
    }

    public static string CreateToJSON(RoomJSON data)
    {
        return JsonUtility.ToJson(data);
    }
}