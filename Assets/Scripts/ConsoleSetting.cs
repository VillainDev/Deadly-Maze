﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleSetting : MonoBehaviour {
    public KeyCode Up = KeyCode.UpArrow;
    public KeyCode Down = KeyCode.DownArrow;
    public KeyCode Left = KeyCode.LeftArrow;
    public KeyCode Right = KeyCode.RightArrow;
    public KeyCode Jump = KeyCode.K;
    public KeyCode Shoot = KeyCode.J;
    public KeyCode ChangeMode = KeyCode.I; //walk mode + aim mode
    public KeyCode ChangeWeapon = KeyCode.L;
    /// <summary>
    /// Bag item will show user items in a canvas
    /// user can choose those items by clicking on these
    /// </summary>
    public KeyCode OpenBagItem = KeyCode.Tab;
    public KeyCode ChangeItemUp = KeyCode.Home;
    public KeyCode ChangeItemDown = KeyCode.End;
    public KeyCode SetPole = KeyCode.P;
    public KeyCode ViewPoleMap = KeyCode.M;
}
