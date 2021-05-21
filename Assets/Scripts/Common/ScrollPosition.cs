using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollPosition : MonoBehaviour
{
    static public List<ScrollPosition> Items = new List<ScrollPosition>();

    private void Awake()
    {
        //Debug.Log(name + "생성됨");
        Items.Add(this);
    }

    private void Start()
    {
        speed *= GameManager.instance.scrollSpeedMultiply;
    }

    private void OnDestroy()
    {
        //Debug.Log(name + "삭제됨");
        Items.Remove(this);
    }


    public float speed = -1f; // 1초에 한칸 왼쪽으로 이동.

    void Update()
    {
        // 스크롤 가로 이동.
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }
}
