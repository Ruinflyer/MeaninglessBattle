using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Meaningless
{
    public class ObjectPool : MonoBehaviour
    {
        [HideInInspector]
        public GameObject prefab;

        public LinkedList<GameObject> usedObjectList;
        public LinkedList<GameObject> unusedObjectList;

        //将对象与池对应,通过对象找到对象池
        private Dictionary<GameObject, ObjectPool> objectPoolDict;

        /// <summary>
        /// 初始化对象池，无Tranform
        /// </summary>
        /// <param name="prefab">对象</param>
        /// <param name="num">对象池大小</param>
        public void InitObjectPool(GameObject prefab, Dictionary<GameObject, ObjectPool> objectPoolDict, int num = 10)
        {
            this.prefab = prefab;
            this.objectPoolDict = objectPoolDict;
            usedObjectList = new LinkedList<GameObject>();
            unusedObjectList = new LinkedList<GameObject>();
            for (int i = 0; i < num; i++)
            {
                GameObject go = GameObject.Instantiate(prefab);
                go.SetActive(false);
                go.transform.SetParent(transform);
                unusedObjectList.AddFirst(go);
                this.objectPoolDict.Add(go, this);
            }
        }

        /// <summary>
        /// 初始化对象池,带初始化Tranform
        /// </summary>
        /// <param name="prefab">对象</param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="num"></param>
        public void InitObjectPool(GameObject prefab, Vector3 position, Quaternion rotation, Dictionary<GameObject, ObjectPool> objectPoolDict, int num = 10)
        {
            this.prefab = prefab;
            this.objectPoolDict = objectPoolDict;
            usedObjectList = new LinkedList<GameObject>();
            unusedObjectList = new LinkedList<GameObject>();
            for (int i = 0; i < num; i++)
            {
                GameObject go = GameObject.Instantiate(prefab, position, rotation);
                go.SetActive(false);
                go.transform.SetParent(transform);
                unusedObjectList.AddFirst(go);
                this.objectPoolDict.Add(go, this);
            }
        }

        /// <summary>
        /// 从对象池中取出对象
        /// </summary>
        /// <returns>返回对象位置</returns>
        public Transform PullObject()
        {
            GameObject go = null;
            if (unusedObjectList.Count > 0)
            {
                go = unusedObjectList.First.Value;
                go.SetActive(true);
                unusedObjectList.RemoveFirst();
                usedObjectList.AddLast(go);
            }
            else
            {
                go = GameObject.Instantiate(prefab);
                go.SetActive(transform);
                usedObjectList.AddLast(go);
                objectPoolDict.Add(go, this);
            }
            return go.transform;
        }

        /// <summary>
        /// 从对象池中取出对象
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public Transform PullObject(Vector3 position, Quaternion rotation)
        {
            GameObject go = null;
            if (unusedObjectList.Count > 0)
            {
                go = unusedObjectList.First.Value;
                go.transform.position = position;
                go.transform.rotation = rotation;
                go.SetActive(true);
                unusedObjectList.RemoveFirst();
                usedObjectList.AddLast(go);
            }
            else
            {
                go = GameObject.Instantiate(prefab);
                go.transform.position = position;
                go.transform.rotation = rotation;
                go.SetActive(transform);
                usedObjectList.AddLast(go);
                objectPoolDict.Add(go, this);
            }
            return go.transform;
        }

        /// <summary>
        /// 将对象放回对象池
        /// </summary>
        /// <param name="handleObject"></param>
        public void PushObject(GameObject handleObject)
        {
            if(handleObject.activeSelf)
            {
                handleObject.SetActive(false);
                unusedObjectList.AddFirst(handleObject);
                usedObjectList.Remove(handleObject);
            }
        }
    }
}