using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ScreenGuiTest))]
public class EditorGUI_ScreenGuiTest : Editor
{
    void OnSceneGUI()
    {
        ScreenGuiTest item = (ScreenGuiTest)target;
        Transform tr = item.transform;
        item.viewingDistance = (float)Handles.ScaleValueHandle(item.viewingDistance
            , tr.position + tr.forward * item.viewingDistance
            , tr.rotation, 1, Handles.ConeHandleCap, 1);
    }
}
#endif

public class ScreenGuiTest : MonoBehaviour
{
    public float viewingDistance = 3;
    public float viewingAngle = 90f;

    private void OnDrawGizmos()
    {
        ScreenGuiTest item = this;
        Transform tr = item.transform;
        Handles.color = Color.red;
        float halfAngle = item.viewingAngle * 0.5f;

        //// 아크 그리기
        Handles.DrawWireArc(tr.position, tr.up
            , tr.forward.AngleToYDirection(-halfAngle), item.viewingAngle
            , item.viewingDistance);

        item.viewingDistance = (float)Handles.ScaleValueHandle
            (item.viewingDistance, tr.position + tr.forward * item.viewingDistance
            , tr.rotation, 1, Handles.ConeHandleCap, 1);

        // 아크의 왼쪽 오른쪽 직선 그리기
        Handles.DrawLine(tr.position
            , tr.forward.AngleToYDirection(-halfAngle) * item.viewingDistance);// 왼쪽선 그리기.
        
        Handles.DrawLine(tr.position
            , tr.forward.AngleToYDirection(halfAngle) * item.viewingDistance); // 오른쪽선 그리기.



        // 문자열 그리기
        GUIStyle style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.red;
        Vector3 namePos = tr.position;
        Handles.Label(namePos, "텍스트", style);
    }
}
