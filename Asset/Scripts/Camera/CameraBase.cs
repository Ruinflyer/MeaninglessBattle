﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class CameraBase : MonoSingleton<CameraBase>
{
    public bool isFollowing=false;

    public float moveSpeed = 120f;
    public GameObject cameraFollowGO;
    public float clampAngle = 80f;
    public float inputSensitivity = 150f;
    public GameObject mainCamera;
    public GameObject player;
    public float canDistanceXToPlayer;
    public float canDistanceYToPlayer;
    public float canDistanceZToPlayer;
    private float mouseX;
    private float mouseY;
    private float rotY = 0f;
    private float rotX = 0f;

    private void Start()
    {
        FindPlayer();
        Vector3 rot = transform.localRotation.eulerAngles;
        rotX = rot.x;
        rotY = rot.y;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        isFollowing = true;
    }

    private void Update()
    {
        if(isFollowing)
        {
            Follow();
        } 
        else
        {
           Cursor.lockState = CursorLockMode.None;
           Cursor.visible = true;
        }
    }

    public void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (!(player.transform.Find("CameraFollow")))
        {
            cameraFollowGO = new GameObject("CameraFollow");
            cameraFollowGO.transform.SetParent(player.transform);
            cameraFollowGO.transform.localPosition = new Vector3(0.75f, 1.25f, 0);
        }
        else
            cameraFollowGO = player.transform.Find("CameraFollow").gameObject;
    }



    private void Follow()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        rotY += mouseX * inputSensitivity * Time.deltaTime;
        rotX -= mouseY * inputSensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = localRotation;
        player.transform.rotation = Quaternion.Euler(0, rotY, 0);
    }

    private void LateUpdate()
    {
        float step = moveSpeed * Time.deltaTime;
        if (isFollowing)
        {
            transform.position = Vector3.MoveTowards(transform.position, cameraFollowGO.transform.position, step);
        } 
    }
}
