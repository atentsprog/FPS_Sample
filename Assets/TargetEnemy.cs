using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public partial class TargetEnemy : MonoBehaviour
{
    public bool handle1;
    public bool handle2;
    public bool handle3;
    public Vector3 direction;
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
                if (agent.remainingDistance < 0.1f)
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

                    Vector3 targetDir = player.position - transform.position;
                    targetDir.Normalize();
                    float angle = Vector3.Angle(targetDir, transform.forward);
                    if (Mathf.Abs(angle) <= viewingAngle * 0.5f)
                    {
                        insideViewingAngle = true;
                    }

                    if (insideViewingAngle)
                    { 
                        Debug.LogWarning("찾았다 -> 추적 상태로 전환");
                    }
                }

                yield return null;
            }
            wayPointIndex++;
        }
    }
}
