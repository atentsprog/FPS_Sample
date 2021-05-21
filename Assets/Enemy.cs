using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;


#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Enemy))]
public class EditorGUI_Enemy : Editor
{
    void OnSceneGUI()
    {
        Enemy item = (Enemy)target;
        Transform tr = item.transform;
        Handles.color = Color.red;

        // 탐색 가능 거리 표시
        DrawViewingAngle(item, tr);


        // 지금 이동 하는 타겟 위치로 직선 긋기.
        DrawMoveToTarget(item, tr);

        // 지금 이동 하는 NavAgent의 위치에 경로 표시하기.
    }


    private static void DrawMoveToTarget(Enemy item, Transform tr)
    {
        if (item.moveToTarget != null)
            Handles.DrawLine(tr.position, item.moveToTarget.position);
    }

    private static void DrawViewingAngle(Enemy item, Transform tr)
    {
        float halfAngle = item.viewingAngle * 0.5f;

        // 아크 그리기
        Handles.DrawWireArc(tr.position, tr.up, tr.forward.AngleToYDirection(-halfAngle), item.viewingAngle, item.viewingDistance);
        item.viewingDistance = (float)Handles.ScaleValueHandle(item.viewingDistance, tr.position + tr.forward * item.viewingDistance, tr.rotation, 1, Handles.ConeHandleCap, 1);

        // 아크의 왼쪽 오른쪽 직선 그리기
        Handles.DrawLine(tr.position, tr.forward.AngleToYDirection(-halfAngle) * item.viewingDistance);     // 왼쪽선 그리기.
        Handles.DrawLine(tr.position, tr.forward.AngleToYDirection(halfAngle) * item.viewingDistance);      // 오른쪽선 그리기.
    }
}
#endif


public class Enemy : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    private float moveSpeed;

    public float viewingDistance = 3;
    public float viewingAngle = 90;

    /// 상태 1) 페트롤 : 지정된 웨이 포인트 이동, 
    ///     로밍이 끝나는 탐색 조건 : 
    ///         1. 시야 범위 안에 적이 들어 옴 ->  추적으로 전환.
    ///         2. 소리 듣는 범위 안에서 총소리 발생하면 해당 방향으로 이동 -> 지정위치 이동으로 전환
    /// 상태 2) 추적 : 추적 대상을 공격 가능한 거리보다 가까워질때까지 추적.
    ///     끝나는 경우 :
    ///         추적 대상이 지정한 범위안에 들어옴 -> 공격으로 전환
    /// 상태 3) 지정위치 이동:
    ///     로밍이 끝나는 것과 같은 탐색 조건 실행
    ///     지정 위치까지 이동 -> 주변 탐색으로 전환
    /// 상태 4) 주변 탐색 -> 360도 공격 대상 탐색
    ///     공격 대상 있다면 
    ///         Yes : 추적으로 전환
    ///         No : 원래 위치 복귀로 전환.
    /// 상태 5) 원래 위치 복귀
    ///     로밍이 끝난지점으로 이동
    ///     로밍이 끝나는 것과 같은 탐색 조건 실행
    ///     지정한 위치 이동후 다음 웨이 포인트로 이동
    /// 상태 6) 공격 : 대상을 향해 공격.
    /// 
    /// 사태 7) 피격 : 공격당했다. 
    ///     hp가 남았는가? 
    ///         Yes : 피격 모션 재생 -> 추적으로 전환
    ///         No : 죽는 모션 재생 -> FSM종료, 파괴
    ///         
    public enum FsmState
    {
        Petrol,             // 페트롤
        Chase,              // 추적
        MoveToLocation,     // 지정위치 이동
        SearchNeardistance, // 근거리 탐색
        ReturnPetrol,       // 원래 위치 복귀
        Attack,             // 공격
        Attacked            // 피격
    }
    //Dictionary<FsmState, Func<IEnumerator>> fsmAction = new Dictionary<FsmState, Func<IEnumerator>>();

    //public bool findedTarget = false;
    //private void Update()
    //{
    //    findedTarget = false;
    //    // 거리안에 있는지 확인.
    //    float distance = Vector3.Distance(transform.position, target.position);
    //    if (distance < viewingDistance)
    //    {
    //        // 각도 안에 있는지 확인
    //        Vector3 targetDir = target.position - transform.position;
    //        targetDir.Normalize();
    //        float angle = Vector3.Angle(targetDir, transform.forward);
    //        if (Mathf.Abs(angle) <= viewingAngle * 0.5f)
    //            findedTarget = true;
    //    }
    //}
    IEnumerator Start()
    {
        moveSpeed = agent.speed;

        //fsmAction[FsmState.Petrol               ] = PetrolCo;
        //fsmAction[FsmState.Chase                ] = ChaseCo;
        //fsmAction[FsmState.MoveToLocation       ] = MoveToLocationCo;
        //fsmAction[FsmState.SearchNeardistance   ] = SearchNeardistanceCo;
        //fsmAction[FsmState.ReturnPetrol         ] = ReturnPetrolCo;
        //fsmAction[FsmState.Attack               ] = AttackCo;
        //fsmAction[FsmState.Attacked             ] = AttackedCo;

        while (true)
        {

            //agent.destination = target.transform.position;
            yield return StartCoroutine(PetrolCo()); // 걸어 다니다
        }
    }


    public List<Transform> wayPoints;
    public int currentWayPointIndex = 0;
    public Transform moveToTarget;
    /// 상태 1) 로밍 : 지정된 웨이 포인트 이동, 
    ///     로밍이 끝나는 탐색 조건 : 
    ///         1. 시야 범위 안에 적이 들어 옴 ->  추적으로 전환.
    ///         2. 소리 듣는 범위 안에서 총소리 발생하면 해당 방향으로 이동 -> 지정위치 이동으로 전환
    private IEnumerator PetrolCo()
    {
        // 지정된 웨이 포인트들을 순회 하면서 무한히 이동.
        while (true)
        {
            currentWayPointIndex++;
            if (currentWayPointIndex >= wayPoints.Count)
                currentWayPointIndex = 0;
            moveToTarget = wayPoints[currentWayPointIndex];

            do
            {
                agent.destination = moveToTarget.position;
                yield return null;

                //시야 범위 안에 적이 있는가?
                if (target)
                {

                    // 거리안에 있는지 확인.
                    float distance = Vector3.Distance(transform.position, target.position);
                    if (distance < viewingDistance)
                    {
                        // 각도 안에 있는지 확인
                        Vector3 targetDir = target.position - transform.position;
                        targetDir.Normalize();
                        float angle = Vector3.Angle(targetDir, transform.forward);
                        if (Mathf.Abs(angle) <= viewingAngle * 0.5f)
                        {
                            //Fsm을 추적으로 전환.
                        }
                    }
                }

            } while (agent.remainingDistance > 1);
        }
    }
    private IEnumerator ChaseCo() { yield return null; }
    private IEnumerator MoveToLocationCo() { yield return null; }
    private IEnumerator SearchNeardistanceCo() { yield return null; }
    private IEnumerator ReturnPetrolCo() { yield return null; }
    private IEnumerator AttackCo() { yield return null; }
    private IEnumerator AttackedCo() { yield return null; }

    #region 피격
    public GameObject attackedEffect;
    public GameObject dieEffect;
    public int hp = 3;

    /// <summary>
    /// 몬스터가 총알에 맞으면 호출된다
    /// </summary>
    internal void OnHit(Vector3 피격된위치)
    {
        //hp를 깎자.
        hp--;

        // 맞았다는 이펙트를 생성하자

        Debug.Log($"{name}의 체력이 {hp}가 되었다");
        if (hp > 0)
        {
            Instantiate(attackedEffect, 피격된위치, Quaternion.identity);

            // 움직이는것을 일시 중지 시키자.
            StartCoroutine(OnHitCo());
        }
        else
        { 
            //죽자. -> 없애자.
            Destroy(gameObject);
            Instantiate(dieEffect, transform.position, Quaternion.identity);
        }
    }

    private IEnumerator OnHitCo()
    {
        agent.speed = 0;
        //// 0.3초 쉬다가 다시 움직이게 하자.
        yield return new WaitForSeconds(0.3f);
        agent.speed = moveSpeed;
    }
    #endregion 피격
}
