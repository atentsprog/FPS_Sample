using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoTest : MonoBehaviour
{
    public float distance = 1;
    public bool DrawSphere = true;
    public bool DrawWireSphere = true;
    public bool DrawWireCube = true;
    public bool DrawRay = true;
    public bool DrawIcon = true;
    public bool DrawFrustum = true;

    //private void OnDrawGizmosSelected() // 선택되었을때 표시하는 기즈모

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        if (DrawSphere)
            Gizmos.DrawSphere(transform.position, distance);

        if (DrawWireSphere)
            Gizmos.DrawWireSphere(transform.position, distance);

        //Vector3.one==new Vector3(1, 1, 1)
        if (DrawWireCube)
            Gizmos.DrawWireCube(transform.position, Vector3.one * distance);

        if (DrawRay)
            Gizmos.DrawRay(transform.position, transform.forward);

        if (DrawIcon)
            Gizmos.DrawIcon(transform.position, "testIcon.png");

        if (DrawFrustum)
            Gizmos.DrawFrustum(transform.position, 60, distance, 1, 1.5f);
                                            // 위치, 각도(fov), 최대거리, 최소 거리, 
    }
}
