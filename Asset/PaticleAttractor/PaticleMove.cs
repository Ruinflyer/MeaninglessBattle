using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class PaticleMove : MonoBehaviour
{
    public int moveSpeed;
    public int destoryTime;
    private NetPoolManager NetPoolManager;
    private Rigidbody RB;

    void Start()
    {
        NetPoolManager = new NetPoolManager();
        RB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move(moveSpeed);
        Timer.Instance.StartCountdown(destoryTime, new TimerEnd(DestroyObject));
    }

    void DestroyObject()
    {
        NetPoolManager.Destroy(gameObject);
    }

    private void Move(float speed)
    {
        RB.AddForce(transform.forward * speed);
    }
}
