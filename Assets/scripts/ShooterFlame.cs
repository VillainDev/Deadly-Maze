using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterFlame : MonoBehaviour
{
    public GameObject obj;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            //tạo đường thẳng từ súng tới trước
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Finish")
            {
                Instantiate(obj, hit.point, Quaternion.identity);
            }
        }

    }
}

