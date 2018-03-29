using UnityEngine;
using System.Collections;

namespace Meaningless
{   
    //不继承于Mono的单例模式
    public class Singleton<T> where T:new()
    {
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance==null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
        protected Singleton()
        {

        }
    }
    //继承于Mono的单例模式
    public class MonoSingleton<T>:MonoBehaviour where T:Component
    {
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance==null)
                {
                    GameObject go = GameObject.Find(typeof(T).Name);
                    if (go==null)
                    {
                        Debug.LogError("场景里面找不到名为"+ typeof(T).Name+"的物体");
                        return null;
                    }
                    _instance = go.GetComponent<T>();
                }
                return _instance;
            }
        }
        protected MonoSingleton()
        {

        }
    }
    /// <summary>
    /// 继承于Mono的单例模式,自动创建GameObject,本身挂载为组件,Don'tDestroyOnLoad
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Mono_DDOLSingleton<T> : MonoBehaviour where T : Component
    {
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = GameObject.Find(typeof(T).Name);
                    if (go == null)
                    {
                        go = new GameObject(typeof(T).Name);
                        GameObject.DontDestroyOnLoad(go);
                        Debug.Log("场景里面找不到名为" + typeof(T).Name + "的物体，已自动创建,并使 "+typeof(T).Name+" 加入DontDestroyOnLoad类别");
                        
                    }
                    _instance = go.AddComponent<T>();
                    Debug.Log("已将"+typeof(T).Name + "的组件自动挂载在为以此类命名的物体上");
                }
                return _instance;
            }
        }
        protected Mono_DDOLSingleton()
        {

        }
    }
}
