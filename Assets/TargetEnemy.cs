using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetEnemy : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    private float moveSpeed;
    public List<Transform> wayPoints;

    public Animator animator;
    public int wayPointIndex = 0;
    // 타겟을 매프레임 쫒아가자.
    IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        moveSpeed = agent.speed;

        // 첫번째 웨이 포인트로 가자.
        animator.Play("run");
        while (true)
        {
            agent.destination = wayPoints[wayPointIndex].position;
            while (true)
            {
                if (agent.remainingDistance == 0)
                {
                    Debug.Log("도착");
                    // 2번째 웨이 포인트로 이동.
                    wayPointIndex++;
                }
                else
                {
                    // 기다리자.
                }
                //yield return null;
            }
        }
    }
    private IEnumerator ChangeSpeed(float stopTime)
    {
        agent.speed = 0;
        yield return new WaitForSeconds(stopTime);
        agent.speed = moveSpeed;
    }

    public GameObject attackedEffect;
    public GameObject destroyEffect;
    // 총알에 맞으면 잠시 0.3초 멈추자.
    public int hp = 3;

    internal void OnHit()
    {
        Debug.Log("OnHit;" + name, transform);
        hp--;

        if( hp > 0)
        {
            StartCoroutine(ChangeSpeed(0.3f));
            // 총알 맞을때 이펙트 보여주자.
            Instantiate(attackedEffect, transform.position, transform.rotation);
        }
        else
        {
            // HP를 추가해서 3대 맞으면 폭팔.
            Instantiate(destroyEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
