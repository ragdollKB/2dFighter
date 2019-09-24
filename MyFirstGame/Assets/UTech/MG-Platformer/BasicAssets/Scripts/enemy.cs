using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public Transform Player;
     // Use this for initialization
     void Start () {
         
     }
     
     // Update is called once per frame
     void Update ()
         
     {
 
         GetComponent<UnityEngine.AI.NavMeshAgent>().destination = Player.position;
     }
}
 
