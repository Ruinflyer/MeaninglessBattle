﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(ResourcesManager.Instance.LoadMagic());
        StartCoroutine(ResourcesManager.Instance.LoadItems());
       
        ResourcesManager.Instance.LoadUITextures();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
