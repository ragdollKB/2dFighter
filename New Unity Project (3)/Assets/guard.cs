using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class guard : MonoBehaviour
{
    public GameObject player;
    private NavMeshAgent navmesh;
    // Start is called before the first frame update
    void Start()
    {
        navmesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        navmesh.destination = player.transform.position;    
    }
}
