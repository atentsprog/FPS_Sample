using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TargetEnemy))]
public class EditorGUI_TargetEnemy : Editor
{
    void OnSceneGUI()
    {
        if (EditorOption.Options[OptionType.디버그_라인그리기] == false)
            return;

        TargetEnemy item = (TargetEnemy)target;
        Transform tr = item.transform;
        item.viewingDistance = (float)Handles.ScaleValueHandle(item.viewingDistance
            , tr.position + tr.forward * item.viewingDistance
            , tr.rotation, 1, Handles.ConeHandleCap, 1);

        if (item.handle1)
        {
            // 로테이션 핸들 1)
            item.viewingAngle = Handles.ScaleSlider(item.viewingAngle, tr.position + tr.forward * item.viewingDistance, tr.right, Quaternion.identity, 2, 0.1f);
        }

        if (item.handle2)
        {
            // 로테이션 핸들 2)
            Quaternion rotate = Quaternion.Euler(new Vector3(0, item.viewingAngle, 0));
            rotate = Handles.FreeRotateHandle(rotate, tr.position, item.viewingDistance);
            item.viewingAngle = rotate.eulerAngles.y;
        }

        if (item.handle3)
        {
            //로테이션 핸들 3)
            Quaternion rotate = Quaternion.Euler(new Vector3(0, item.viewingAngle, 0));
            rotate = Handles.RotationHandle(rotate, tr.position);
            item.viewingAngle = rotate.eulerAngles.y;
        }
    }
}