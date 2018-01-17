using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Meaningless;
using UnityEngine.SceneManagement;

public class GameTool : MonoBehaviour {

    //游戏工具类，为了方便外界调用，里面所有的方法都设置为静态方法

    ////清理内存（一般在切换场景的时候调用）
    public static void ClearMemory()
    {
        GC.Collect();//垃圾回收
        Resources.UnloadUnusedAssets();//卸载内存中没用的资源
    }
    //分割字符串
    public static string[] SplitString(string str,char c)
    {
        string[] arr = str.Split(c);
        return arr;
    }
    //PlayerPrefs的封装
    //判断系统内存中是否存在某个键
    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
    //取值
    public static int GetInt(string key)
    {  
        return PlayerPrefs.GetInt(key);
    }
    public static float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }
    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key);
    }
    //存值
    public static void SetInt(string key,int value)
    {
        PlayerPrefs.SetInt(key,value);
    }
    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }
    //清除内存中存储的值
    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
    //清除内存中指定的键
    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
    //查找父物体下面的子物体
    public static Transform FindTheChild(GameObject goParent,string  childName)
    {
        Transform searchTrans = goParent.transform.Find(childName);
        if (searchTrans==null)
        {
            foreach (Transform trans in goParent.transform)
            {
                searchTrans = FindTheChild(trans.gameObject, childName);
                if (searchTrans!=null)
                {
                    return searchTrans;
                }
            }
        }
        return searchTrans;
    }
    //查找父物体下面所有的同名子物体
    static List<Transform> listTrans = new List<Transform>();
    public static List<Transform> FindTheChilds(GameObject goParent, string childName,bool isNewList=true)
    {
        if (isNewList)
        {
            listTrans.Clear(); 
        }
        for (int i = 0; i < goParent.transform.childCount; i++)
        {
            Transform searchTrans = goParent.transform.GetChild(i);
            Debug.Log(searchTrans.name);
            if (searchTrans.name==childName)
            {
                listTrans.Add(searchTrans);
            }
            FindTheChilds(searchTrans.gameObject, childName,false);
        }
        return listTrans;
    }
    //获取父物体下面的子物体上面的组件
    public static T GetTheChildComponent<T>(GameObject goParent, string childName) where T : Component
    {
        Transform searchTrans = FindTheChild(goParent, childName);
        if (searchTrans!=null)
        {
            return searchTrans.GetComponent<T>();
        }
        else
        {
            return null;
            //return default(T);
        }
    }
    //获取父物体下面所有同名子物体上面的组件
    public static List<T> GetTheChildsComponent<T>(GameObject goParent, string childName) where T : Component
    {
        List<T> componentList = new List<T>();
        List<Transform> listTrans = FindTheChilds(goParent,childName);
        for (int i = 0; i < listTrans.Count; i++)
        {
            if (listTrans!=null)
            {
                T t= listTrans[i].GetComponent<T>();
                if (t != null)
                {
                    componentList.Add(listTrans[i].GetComponent<T>());
                }
                else
                {
                    Debug.LogError("在子物体上面没找到"+typeof(T).Name+"组件");
                }
            }
        }
        return componentList;
    }
    //添加组件给父物体下面的子物体
    public static T AddTheChildComponent<T>(GameObject goParent, string childName) where T : Component
    {
        Transform searchTrans = FindTheChild(goParent,childName);
        if (searchTrans != null)
        {
            T[] arr = searchTrans.GetComponents<T>();
            for (int i = 0; i < arr.Length; i++)
            {
                Destroy(arr[i]);
            }
            return searchTrans.gameObject.AddComponent<T>();
        }
        else
        {
            return null;
        }
    }
    //添加组件给父物体下面的所有同名子物体
    public static List<T> AddChildsComponent<T>(GameObject goParent, string childName) where T : Component
    {
        List<T> listComponent = new List<T>();
        List<Transform> listTrans = FindTheChilds(goParent,childName);
        if (listTrans!=null)
        {
            for (int i = 0; i < listTrans.Count; i++)
            {
                T[] arr = listTrans[i].GetComponents<T>();
                for (int j = 0; j < arr.Length; j++)
                {
                    Destroy(arr[j]);
                }
                T t= listTrans[i].gameObject.AddComponent<T>();
                listComponent.Add(t);
            }
        }
        return listComponent;
    }
    //添加子物体的方法
    public static void AddChildToParent(Transform parentTrans,Transform childTrans)
    {
        childTrans.parent = parentTrans;
        childTrans.localPosition = Vector3.zero;
        childTrans.localScale = Vector3.one;
    }
   
}
