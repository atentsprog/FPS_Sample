using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    IEnumerator Start()
    {
        while(true)
        { 
            agent.destination = target.transform.position;
            yield return null;
        }
    }
}
