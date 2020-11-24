using System;
using System.Collections.Generic;

[System.Serializable]
public class Player
{
    public string UUID;
    public float Pos_x;
    public float Pos_y;
    public float Pos_z;
}


[System.Serializable]
public class ObjectStatus
{
    public string Types;

    public string UUID;

    public Player player1;
    public Player player2;
}

