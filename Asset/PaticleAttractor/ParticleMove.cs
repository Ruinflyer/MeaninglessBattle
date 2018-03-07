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

    void Start()
    {
        NetPoolManager = new NetPoolManager();
        RB = GetComponent<Rigidbody>();
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
        //Timer.Instance.StartCountdown(destoryTime, new TimerEnd(DestroyObject));
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
