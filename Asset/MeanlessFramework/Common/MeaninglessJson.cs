using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Meaningless
{
    public class MeaninglessJson
    {
      
        //Resource文件夹路径
        public static string Path_Res = Application.dataPath + "/Resources/";
        //StreamingAssets文件夹路径
        public static string Path_StreamingAssets = Application.dataPath + "/StreamingAssets/";

        /// <summary>
        /// 从文件加载Json反序列化
        /// </summary>
        /// <typeparam name="T">序列化后的类</typeparam>
        /// <param name="JsonFilePath">Json文件路径</param>
        /// <returns></returns>
        public static T LoadJsonFromFile<T>(string JsonFilePath)
        {
            if(!File.Exists(JsonFilePath))
            {
                Debug.LogError("加载Json文件 "+JsonFilePath+" 失败,文件不存在");
                return default(T);
            }

            FileStream jsonFileStream = new FileStream(JsonFilePath,FileMode.Open);
            StreamReader jsonFileStreamR = new StreamReader(jsonFileStream,System.Text.Encoding.UTF8);
            string json=jsonFileStreamR.ReadToEnd();
            jsonFileStreamR.Close();
            jsonFileStream.Close();

            if (json.Length>0)
            {
                return JsonUtility.FromJson<T>(json);
                
            }
            else
            {
                Debug.LogError("加载Json文件 " + JsonFilePath + " 失败,文件为空");
                return default(T);
            }
        }
        /// <summary>
        /// 将Json保存为文件
        /// </summary>
        /// <param name="JsonFilePath">文件路径</param>
        /// <param name="json">json内容</param>
        /// <returns></returns>
        public static bool SavaJsonAsFile(string JsonFilePath,string json)
        {
            FileStream jsonFileStream = new FileStream(JsonFilePath, FileMode.Create);
            StreamWriter jsonFileStreamW = new StreamWriter(jsonFileStream, System.Text.Encoding.UTF8);
            jsonFileStreamW.Write(json);
            jsonFileStreamW.Flush();
            jsonFileStreamW.Close();
            jsonFileStream.Close();
            if(File.Exists(JsonFilePath))
            {
                return true;
            }
           else
            {
                return false;
            }

        }
        /// <summary>
        /// 序列化对象转化为Json
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string ToJson(object obj)
        {
            return JsonUtility.ToJson(obj);
        }
        /// <summary>
        /// 读取Json数据来覆写该物件的数据
        /// </summary>
        /// <param name="json">Json数据</param>
        /// <param name="ObjectToOverwrite">要被覆写数据的物件</param>
        public static void FromJsonOverWrite(string json,object ObjectToOverwrite)
        {
            JsonUtility.FromJsonOverwrite(json, ObjectToOverwrite);
        }
        /// <summary>
        /// 使用Json数据反序列化类
        /// </summary>
        /// <typeparam name="T">可以被序列化的类</typeparam>
        /// <param name="json">Json数据</param>
        /// <returns></returns>
        public static T FromJson<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}