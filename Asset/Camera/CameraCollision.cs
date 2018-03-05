using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour {

    public float minDistnce = 1f;
    public float maxDistance = 5f;
    public float smooth = 10;
    public float distance;
    private Vector3 dir;

    private void Awake()
    {
        dir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, transform.parent.TransformPoint(dir * maxDistance), out hit))
        {   
            distance = Mathf.Clamp(hit.distance * 0.9f, minDistnce, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dir * distance, Time.deltaTime * smooth);
    }
}
