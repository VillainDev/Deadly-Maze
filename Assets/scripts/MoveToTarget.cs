using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MoveToTarget : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;//đối tuộng muốn chạy tới
                             // Use this for initialization
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        //lấy cái navigation

    }
    public void MoveToPosition(Vector3 position)
    {
        agent.SetDestination(position);
    }

    // Update is called once per frame
    void Update()
    {
        if((Mathf.Sqrt(Mathf.Abs((transform.position.x-target.transform.position.x)* (transform.position.x - target.transform.position.x)+ (transform.position.z - target.transform.position.z) * (transform.position.z - target.transform.position.z))) <100))
        agent.SetDestination(target.transform.position);//tới chỗ đó



    }
}
