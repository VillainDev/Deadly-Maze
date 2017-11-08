using System.Collections;
using System;
using UnityEngine;


public class TrapArrow : MonoBehaviour {
    public Transform ArrowSeries;
    public Transform Arrow;
    public Transform Arrow1;
    public Transform Arrow2;
    private Vector3 apos;
    public float speedo;
    public float range;
    private void Start () {
        ArrowSeries.gameObject.SetActive (false);
        apos = Arrow.position;
    }
    void Update () {
        ArrowSeries.gameObject.SetActive (true);
        float UpdateRange = Math.Abs(Arrow.position.z - apos.z);
        if (UpdateRange < range) {
            Vector3 offset = new Vector3 (0.0f, 0.0f, speedo / 100);
            Arrow.position += offset;
        } else {
            Arrow.position = apos;
        }
    }
}