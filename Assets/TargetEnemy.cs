using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class TargetEnemy : MonoBehaviour
{
    public Transform player;
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

        yield return StartCoroutine(PetrolCo());
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, viewingDistance);

        // 시야각표시
        // 호 표시.
        //float
        //Transform tr = GetComponent<Transform>(); // 같은 의미
        Transform tr = transform;
        float halfAngle = viewingAngle * 0.5f;
        Handles.DrawWireArc(tr.position, tr.up, tr.forward.AngleToYDirection(-halfAngle)
            , viewingAngle, viewingDistance);

        // 오른쪽 왼쪽 선 표시.
        Handles.DrawLine(tr.position, tr.position + tr.forward.AngleToYDirection(-halfAngle) * viewingDistance);
        Handles.DrawLine(tr.position, tr.position + tr.forward.AngleToYDirection(halfAngle) * viewingDistance);
    }

    public float viewingDistance = 3;
    public float viewingAngle = 90f; // 시야각
    IEnumerator PetrolCo()
    {
        // 첫번째 웨이 포인트로 가자.
        animator.Play("run");
        while (true)
        {
            if (wayPointIndex >= wayPoints.Count)
                wayPointIndex = 0;
            agent.destination = wayPoints[wayPointIndex].position;
            yield return null;
            while (true)
            {
                if (agent.remainingDistance == 0)
                {
                    Debug.Log("도착");
                    // 2번째 웨이 포인트로 이동.
                    break;
                }

                //플레이어 탐지.
                // 플레이어와 나와의 위치를 구하자.
                float distance = Vector3.Distance(transform.position, player.position);
                if (distance < viewingDistance)
                {
                    // 시야각에 들어 왔다면 
                    bool insideViewingAngle = false;
                    // 시야각에 들어왔는지 확인하는 로직 넣자.


                    if(insideViewingAngle)
                    { 
                        Debug.LogWarning("찾았다 -> 추적 상태로 전환");
                    }
                }

                yield return null;
            }
            wayPointIndex++;
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
