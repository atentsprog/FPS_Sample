using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 확장함수 샘플, 함수 이름은 중요하지 않음.
/// </summary>
static public class MyExtension
{
    static public Vector3 AngleToYDirection(this Vector3 baseDirection, float angle)
    {
        var quaternion = Quaternion.Euler(0, angle, 0);
        Vector3 newDirection = quaternion * baseDirection;

        return newDirection;
    }

    static public string ToNumber(this int value)
    {
        return $"{value:N0}";
    }

    /// <summary>
    /// 경로를 리턴하는 함수
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    static public string GetPath(this Transform t)
    {
        // 부모가 있으면 부모 경로와 경로 구분자를 넣는다.
        StringBuilder sb = new StringBuilder();
        GetParentPath(t, sb);
        return sb.ToString();

        void GetParentPath(Transform tr, StringBuilder sb)
        {
            if (tr.parent != null)
            {
                GetParentPath(tr.parent, sb);

                sb.Append(tr.parent.name);
                sb.Append(System.IO.Path.DirectorySeparatorChar);
            }

            sb.Append(tr.name);
        }
    }
}