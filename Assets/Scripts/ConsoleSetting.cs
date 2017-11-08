using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConsoleSetting {
    public static KeyCode Up = KeyCode.UpArrow;
    public static KeyCode Down = KeyCode.DownArrow;
    public static KeyCode Left = KeyCode.LeftArrow;
    public static KeyCode Right = KeyCode.RightArrow;
    public static KeyCode Jump = KeyCode.K;
    public static KeyCode Shoot = KeyCode.J;
    public static KeyCode ChangeMode = KeyCode.I; //walk mode + aim mode
    public static KeyCode ChangeWeapon = KeyCode.L;
    /// <summary>
    /// Bag item will show user items in a canvas
    /// user can choose those items by clicking on these
    /// </summary>
    public static KeyCode OpenItem = KeyCode.Tab;
    public static KeyCode SelectUpperItem = KeyCode.Home;
    public static KeyCode SelectLowerItem = KeyCode.End;
    public static KeyCode SetPole = KeyCode.P;
    public static KeyCode ViewMap = KeyCode.M;
}
