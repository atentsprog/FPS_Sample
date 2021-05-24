using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //움직이기
    //기존 코드 재사용, 보는 방향으로 이동

    //카메라 회전 // 

    //총쏘기
    //트레일 렌더러, 물리로 던지기

    //수류탄 던지기

    public float speed = 0.1f;
    public float mouseSensitivity = 40f;
    public Animator animator;
    public Transform cameraTr;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        cameraTr = transform.Find("Main Camera");
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void CameraRotate()
    {
        // 카메라 로테이션을 바꾸자. -> 마우스 이동량에 따라.
        float mouseMoveX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseMoveY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        //cameraTr
        var worldUp = cameraTr.InverseTransformDirection(Vector3.up);
        var rotation = cameraTr.rotation *
                       Quaternion.AngleAxis(mouseMoveX, worldUp) *
                       Quaternion.AngleAxis(mouseMoveY, Vector3.left);
        transform.eulerAngles = new Vector3(0f, rotation.eulerAngles.y, 0f);
        cameraTr.rotation = rotation;
    }

    private void Update()
    {
        // WASD, W위로, A왼쪽,S아래, D오른쪽
        Move();

        CameraRotate();
    }

    private void Move()
    {
        float moveX = 0;
        float moveZ = 0;
        // || -> or
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))moveZ = 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) moveZ = -1;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) moveX = -1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveX = 1;

        //// 이트렌스폼의 앞쪽으로 움직여야 한다.
        Vector3 move = transform.forward * moveZ + transform.right * moveX;
        move.Normalize();

        transform.Translate(move * speed * Time.deltaTime, Space.World);


        if (animator.GetCurrentAnimatorStateInfo(0).IsName("shoot") == false)
        {
            if (moveX != 0 || moveZ != 0)
                animator.Play("run");
            else
                animator.Play("idle");
        }
    }
}
