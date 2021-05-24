using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public partial class TargetEnemy : MonoBehaviour
{
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
