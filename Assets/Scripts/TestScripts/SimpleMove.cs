using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour {

    private CharacterController cc;
    public float speed;
    private Vector3 mDir;
    private float v, h;
    // Use this for initialization
    void Start () {
        cc = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        
        mDir = transform.TransformDirection(new Vector3(h, 0, v));
        mDir *= speed;
        mDir.y -= 0.98f;
        cc.Move(mDir*Time.deltaTime);
	}
}
