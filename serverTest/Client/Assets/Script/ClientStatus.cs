﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientStatus
{
    public static string UUID { get; set; }
    public static string[] currentUUID = {"", ""};
    //public static string[] currentUUID = new string[2];
    public static string currentingRoom { get; set; }

    public static int[] score = { 0, 0 };
    public static bool GameOver;
}
