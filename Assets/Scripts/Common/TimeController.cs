using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private void Update()
    {
        //Time.deltaTime 사양이 다른 컴퓨터에서도 동일한 결과를 만들기 위해서
        //Time.timeScale == 1 , 2 : 2배빠른 속도, 0.1f : 10배 느린 속도.

        // Z키 누르면 타일 스케일 느려지도록
        if (Input.GetKeyDown(KeyCode.Z))
            Time.timeScale *= 0.5f;

        // C키 누르면 타임 스케일 빨라지도록
        if (Input.GetKeyDown(KeyCode.C))
        {
            //Time.timeScale *= 2f;
            Time.timeScale = Time.timeScale * 2f;
        }

        // x누르면 항상 정속도
        if (Input.GetKeyDown(KeyCode.X))
        {
            Time.timeScale = 1;
        }

        // 왼쪽 컨트롤키 누르면 타임 스케일 정속도, 정속도일땐 0이 되도록(0/1 토글되도록)
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (Time.timeScale == 1)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }
    }
}