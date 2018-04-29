using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCanCheck : MonoBehaviour {

    public List<GameObject> List_CanAttack = new List<GameObject>();
    public GameObject[] List_Enemy;
    public float dis;
    public float angle;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        List_Enemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in List_Enemy)
        {
            if (CheckCanAttack(gameObject, enemy, dis, angle))
            {
                Debug.Log(enemy.name);
            }
        }
        

    }

    public bool CheckCanAttack(GameObject center, GameObject enemy, float distance, float angle)
    {
        float dis = (enemy.transform.position - center.transform.position).magnitude;
        Vector3 relativeVector = enemy.transform.position - center.transform.position;
        float ang = Vector3.Angle(relativeVector, center.transform.forward);

        if (dis <distance&&ang<angle)
        {
            Debug.Log(Mathf.Cos(angle) * distance);
            if (!List_CanAttack.Contains(enemy))
                List_CanAttack.Add(enemy);
            return true;
        }
        else
        {
            if (List_CanAttack.Contains(enemy))
                List_CanAttack.Remove(enemy);
            return false;
        }
    }
}
