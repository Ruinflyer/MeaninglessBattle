using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour {

    public string ResName=null;


    [PunRPC]
	public void Hide()
    {
        gameObject.SetActive(false);
    }

}
