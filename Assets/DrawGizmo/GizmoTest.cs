using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoTest : MonoBehaviour
{
    public Color color = Color.green;

    public float distance = 2;


    public bool drawSphere = true;
    public bool drawWireSphere = true;
    public bool drawCube = true;
    public bool drawLine = true;
    public bool drawFrustum = true;
    public bool drawIcon = true;

    //OnDrawGizmosSelected : 선택했을때만 보이는 함수.
    void OnDrawGizmos() // : 선택하지 않았을때도 기즈모를 보이게 하는 함수.
    {
        Gizmos.color = color;

        // 원 그리기
        if (drawSphere)
            Gizmos.DrawSphere(transform.position, distance);

        if (drawWireSphere)
            Gizmos.DrawWireSphere(transform.position, distance);

        // 박스 그리기
        if (drawCube)
            Gizmos.DrawWireCube(transform.position, Vector3.one * distance);

        // 라인 그리기
        if (drawLine)
            Gizmos.DrawLine(transform.position, transform.forward * distance);

        // 카메라 절두체 그리기
        if (drawFrustum)
            Gizmos.DrawFrustum(transform.position, 45, distance, 1, 1.5f);

        // 아이콘 그리기
        if (drawIcon)
            Gizmos.DrawIcon(transform.position, "ninja.png", true);
    }
}
