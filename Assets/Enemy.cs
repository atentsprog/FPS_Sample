using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    private float moveSpeed;

    IEnumerator Start()
    {
        moveSpeed = agent.speed;

        while (true)
        { 
            agent.destination = target.transform.position;
            yield return null;
        }
    }
    public GameObject attackedEffect;
    public GameObject dieEffect;

    public int hp = 3;
    /// <summary>
    /// 몬스터가 총알에 맞으면 호출된다
    /// </summary>
    internal void OnHit(Vector3 피격된위치)
    {
        if(attackedEffect != null)
            Instantiate(attackedEffect, 피격된위치, Quaternion.identity);

        //hp를 깎자.
        hp--;

        // 맞았다는 이펙트를 생성하자

        Debug.Log($"{name}의 체력이 {hp}가 되었다");
        if(hp <= 0)
        {
            //죽자. -> 없애자.
            Destroy(gameObject);
            Instantiate(dieEffect, transform.position, Quaternion.identity);
        }

        // 움직이는것을 일시 중지 시키자.
        StartCoroutine(OnHitCo());
    }

    private IEnumerator OnHitCo()
    {
        agent.speed = 0;
        //// 1초 쉬다가 다시 움직이게 하자.
        yield return new WaitForSeconds(1);
        agent.speed = moveSpeed;
    }
}
