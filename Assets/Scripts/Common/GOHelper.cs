using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOHelper : MonoBehaviour
{
    /// <summary>
    /// 0보다 클경우 활성화후 destroyTime초 이후 파괴됨
    /// </summary>
    public float destroyTime;

    public bool OnEnableLog;
    public bool OnDisableLog;
    public bool OnDestroyLog;
    public bool StopOnLog; // 로그 발생시 멈춤

    // 업데이트 직전에
    private void Start()
    {
        if (destroyTime > 0)
            Destroy(gameObject, destroyTime);
    }

    // 게임 오브젝트 활성화될때 
    private void OnEnable()
    {
        if (OnDestroyLog)
            WriteLog("OnEnable");
    }

    // 게임 오브젝트 비활성화될때
    private void OnDisable()
    {
        if (OnDisableLog)
            WriteLog("OnDisable");
    }

    // 게임 오브젝트 파괴될때
    private void OnDestroy()
    {
        if (OnDestroyLog)
            WriteLog("OnDestroy");
    }

    private void WriteLog(string log)
    {
        if (StopOnLog)
            Debug.LogError(log + ":" + transform.GetPath(), transform);
        else
            Debug.Log(log + ":" + transform.GetPath());
    }
}