using UnityEngine;
using System;
using System.Collections;

public class TrapMace : MonoBehaviour {
    public Transform Mace;
    public Transform Mace1;
    public Transform Mace2;
    public Transform Mace3;
    private Vector3 mpos;
    private Vector3 mpos1;
    private Vector3 mpos2;
    private Vector3 mpos3;
    /// <summary>
    /// Special tools for rotating the maces
    /// </summary>
    private float timeCounter = 0;
    public float speed;
    public float deltaTime;
    private void Start() {
        mpos = Mace.position;
        mpos1 = Mace1.position;
        mpos2 = Mace2.position;
        mpos3 = Mace3.position;
    }
    private void Update() {
        timeCounter += Time.deltaTime * speed / 20;
        float x = Mathf.Sin(timeCounter) * 10;
        float x1 = Mathf.Sin(timeCounter + deltaTime) * 10;

        Mace.position = new Vector3 (x + mpos.x, mpos.y, mpos.z);
        Mace1.position = new Vector3 (-x1 + mpos1.x, mpos1.y, mpos1.z);
        Mace2.position = new Vector3 (x1 + mpos2.x, mpos2.y, mpos2.z);
        Mace3.position = new Vector3 (-x + mpos3.x, mpos3.y, mpos3.z);
    }
}