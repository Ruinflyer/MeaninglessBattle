using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class ParticleMove : MonoBehaviour
{
    public int moveSpeed;
    public int destoryTime;
    private NetPoolManager NetPoolManager;
    private Rigidbody RB;
    float time = 0;
    RaycastHit hitInfo;
    Vector3 targetPoint;
    void Start()
    {
        NetPoolManager = new NetPoolManager();
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out hitInfo))
        {
            targetPoint = hitInfo.point;
            Debug.Log(hitInfo.collider);
        }
        else
        {
            targetPoint = Camera.main.transform.forward * 100;
        }
        transform.LookAt(targetPoint);
    }

    private void FixedUpdate()
    {
        Move(moveSpeed);
        time += Time.fixedDeltaTime;
        if(time>destoryTime)
        {
            DestroyObject();
            time = 0;
        }
    }

    void DestroyObject()
    {
        NetPoolManager.Destroy(gameObject);
    }

    private void Move(float speed)
    {
        transform.Translate(0, 0, speed * Time.fixedDeltaTime, Space.Self);
      
        
    }
}
