using System.Collections.Generic;
using System.Collections;
using UnityEngine;


/// <summary>
/// 可序列化类-Bundle配置
/// </summary>
[System.Serializable]
public class BundleConf
{
    //资源名
    public string[] ResName;
    //AssetBundle的相对路径
    public string BundlePath;
}

namespace Meaningless
{
    public class AssetBundleLoader
    {
        /// <summary>
        /// 读取存放Bundle配置的JSON文件，通过同步方式加载Bundle，资源存入字典
        /// </summary>
        /// <typeparam name="T">字典的值类型</typeparam>
        /// <param name="BundleConfPath">BundleConf的JSON文件路径，文件夹路径为StreamingAssets文件夹里的相对路径</param>
        /// <param name="dict">Dictionary<string,T>，键为string，值为T类型的字典</param>
        public static void LoadBundleToDict<T>(string BundleConfPath, Dictionary<string, T> dict) where T : Object
        {
            BundleConf BundleConf = new BundleConf();
            BundleConf = MeaninglessJson.LoadJsonFromFile<BundleConf>(MeaninglessJson.Path_StreamingAssets + BundleConfPath);

            AssetBundle AB = AssetBundle.LoadFromFile(Application.dataPath + "/StreamingAssets/" + BundleConf.BundlePath);
            
            foreach (string str in BundleConf.ResName)
            {
                dict.Add(str, AB.LoadAsset<T>(str));
            }
        }
        /// <summary>
        /// 读取存放Bundle配置的JSON文件，通过异步方式加载Bundle，资源存入字典
        /// </summary>
        /// <typeparam name="T">字典的值类型</typeparam>
        /// <param name="BundleConfPath">BundleConf的JSON文件路径，文件夹路径为StreamingAssets文件夹里的相对路径</param>
        /// <param name="dict">Dictionary<string,T>，键为string，值为T类型的字典</param>
        public static IEnumerator LoadBundleToDictAsync<T>(string BundleConfPath, Dictionary<string, T> dict) where T : Object
        {

            BundleConf BundleConf = new BundleConf();
            BundleConf = MeaninglessJson.LoadJsonFromFile<BundleConf>(MeaninglessJson.Path_StreamingAssets + BundleConfPath);
            AssetBundleCreateRequest createRequest = AssetBundle.LoadFromFileAsync(Application.dataPath + "/StreamingAssets/" + BundleConf.BundlePath);
            yield return createRequest;
            
            if (createRequest.isDone)
            {
                foreach (string str in BundleConf.ResName)
                {
                    dict.Add(str, createRequest.assetBundle.LoadAsset<T>(str));
                }
            }
        }
    }

}
