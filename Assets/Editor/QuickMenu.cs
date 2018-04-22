using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class QuickMenu : MonoBehaviour {


    [MenuItem("GameObject/返回游戏场景", false, -1)]
    [MenuItem("Assets/返回游戏场景", false, -1)]
    static void BackToMapScene()
    {
        EditorSceneManager.OpenScene("Assets/Scene/Map1.unity");
    }

    [MenuItem("GameObject/返回UI场景", false, -1)]
    [MenuItem("Assets/返回UI场景", false, -1)]
    static void BackToUIScene()
    {
        EditorSceneManager.OpenScene("Assets/Scene/MeaninglessBattle.unity");
    }

}
