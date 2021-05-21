using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    private float moveSpeed;


    //OnDrawGizmosSelected : 선택했을때만 보이는 함수.
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }

    /// 상태 1) 로밍 : 지정된 웨이 포인트 이동, 
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

    IEnumerator Start()
    {
        moveSpeed = agent.speed;

        while (true)
        {

            //agent.destination = target.transform.position;
            yield return StartCoroutine(RoamingCo()); // 걸어 다니다
        }
    }


    public List<Transform> wayPoints;
    int currentWayPointIndex;
    /// 상태 1) 로밍 : 지정된 웨이 포인트 이동, 
    ///     로밍이 끝나는 탐색 조건 : 
    ///         1. 시야 범위 안에 적이 들어 옴 ->  추적으로 전환.
    ///         2. 소리 듣는 범위 안에서 총소리 발생하면 해당 방향으로 이동 -> 지정위치 이동으로 전환
    private IEnumerator RoamingCo()
    {
        // 지정된 웨이 포인트 이동.
        //wayPoints()
        yield return null;

        // 자기 주변 랜덤 탐색.
    }

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
