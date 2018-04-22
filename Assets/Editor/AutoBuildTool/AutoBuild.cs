using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using System.Linq;

public class AutoBuildTemplate
{
    public static string DefineClass_Path = Application.dataPath + "/Scripts/" + "GameDefine.cs";
    public static string DefineClass_Keyword_UIDict = "/*UIDictHere*/";
    public static string DefineClass_Keyword_UIID = "/*UIIDHere*/";
    public static string DefineClass_Keyword_SwitchUIScriptType = "/*SwitchUIScriptType*/";
    #region UIClass
    public static string UIClass =
@"using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Meaningless;
public class #类名# : BaseUI
{
    //AutoStatement
    #成员#
    protected override void InitUiOnAwake()
    {
        #获取组件#
    }
    protected override void InitDataOnAwake()
    {
        this.uiId = UIid.#类名#;
    }
}
";
    #endregion

}


public class AutoBuild
{
    [MenuItem("Meaningless/生成UI代码")]
    public static void BuildUIScript()
    {
        //目录不存在先创建
        if (Directory.Exists(Application.dataPath + "/Scripts/UI/") == false)
        {
            Directory.CreateDirectory(Application.dataPath + "/Scripts/UI/");
        }
        if(Directory.Exists(Application.dataPath+"/Resources/UIPrefab/")==false)
        {
            Directory.CreateDirectory(Application.dataPath + "/Resources/UIPrefab/");
        }

        var dicUIType = new Dictionary<string, string>();
        dicUIType.Add("Img", "Image");
        dicUIType.Add("Btn", "Button");
        dicUIType.Add("Txt", "Text");
        dicUIType.Add("Tran", "Transform");


        GameObject[] selectobjs = Selection.gameObjects;

        foreach (GameObject go in selectobjs)
        {
            //选择的物体
            GameObject selectobj = go.transform.root.gameObject;

            //物体的子物体
            Transform[] _transforms = selectobj.GetComponentsInChildren<Transform>(true);
            List<Transform> childList = new List<Transform>(_transforms);

            //UI需要查询的物体
            var mainNode = from trans in childList where trans.name.Contains('_') && dicUIType.Keys.Contains(trans.name.Split('_')[0]) select trans;
            var nodePathList = new Dictionary<string, string>();

            //循环得到物体路径
            foreach (Transform node in mainNode)
            {
                Transform tempNode = node;
                string nodePath = "/" + tempNode.name;
                while (tempNode != tempNode.root)
                {
                    tempNode = tempNode.parent;
                    int index = nodePath.IndexOf('/');
                    nodePath = nodePath.Insert(index, "/" + tempNode.name);
                }
                nodePathList.Add(node.name, nodePath);
            }


            //成员变量字符串
            string memberstring = "";
            //查询代码字符串
            string loadedcontant = "";

            foreach (Transform itemtran in mainNode)
            {
                string typeStr = dicUIType[itemtran.name.Split('_')[0]];
                memberstring += "private " + typeStr + " " + itemtran.name + " = null;\r\n\t";
                loadedcontant += itemtran.name + " = " + "GameTool.GetTheChildComponent<" + typeStr + ">(this.gameObject," + "\"" + itemtran.name + "\"" + ");\r\n\t\t";

            }

            string scriptPath = Application.dataPath + "/Scripts/UI/" + selectobj.name + ".cs";


            string classStr = "";

            //如果已经存在了脚本，则只替换//auto下方的字符串
            if (File.Exists(scriptPath))
            {
                FileStream classfile = new FileStream(scriptPath, FileMode.Open);
                StreamReader read = new StreamReader(classfile);
                classStr = read.ReadToEnd();
                read.Close();
                classfile.Close();
                File.Delete(scriptPath);

                string splitStr = "//AutoStatement";
                string unchangeStr = Regex.Split(classStr, splitStr, RegexOptions.IgnoreCase)[0];
                string changeStr = Regex.Split(AutoBuildTemplate.UIClass, splitStr, RegexOptions.IgnoreCase)[1];

                StringBuilder build = new StringBuilder();
                build.Append(unchangeStr);
                build.Append(splitStr);
                build.Append(changeStr);
                classStr = build.ToString();
            }
            else
            {
                classStr = AutoBuildTemplate.UIClass;
            }

            classStr = classStr.Replace("#类名#", selectobj.name);
            classStr = classStr.Replace("#获取组件#", loadedcontant);
            classStr = classStr.Replace("#成员#", memberstring);


            FileStream file = new FileStream(scriptPath, FileMode.CreateNew);
            StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8);
            fileW.Write(classStr);
            fileW.Flush();
            fileW.Close();
            file.Close();
            Debug.Log("创建UI脚本 " + Application.dataPath + "/Scripts/UI/" + selectobj.name + ".cs 成功!");
            #region Define类
            //读取DefineClass
            string DefineClassStr = "";
            FileStream DefineClassFileA = new FileStream(AutoBuildTemplate.DefineClass_Path, FileMode.Open);
            StreamReader DefineClassFileR = new StreamReader(DefineClassFileA);
            DefineClassStr = DefineClassFileR.ReadToEnd();
            DefineClassFileR.Close();
            DefineClassFileA.Close();
            

            DefineClassStr =DefineClassStr.Replace(AutoBuildTemplate.DefineClass_Keyword_UIID,
                ","+selectobj.name+"\n\t\t"+AutoBuildTemplate.DefineClass_Keyword_UIID);

            DefineClassStr=DefineClassStr.Replace(AutoBuildTemplate.DefineClass_Keyword_UIDict, 
                "{UIid." + selectobj.name + ",\"UIPrefab/" +selectobj.name+ "\"},\n\t\t\t"+AutoBuildTemplate.DefineClass_Keyword_UIDict);

            DefineClassStr=DefineClassStr.Replace(AutoBuildTemplate.DefineClass_Keyword_SwitchUIScriptType, 
                "case UIid." + selectobj.name + ":\n\t\t\t\t\t scriptType=typeof(" + selectobj.name + ");\n\t\t\t\t\tbreak;\n\t\t\t\t" + AutoBuildTemplate.DefineClass_Keyword_SwitchUIScriptType);

            FileStream DefineClassFileB = new FileStream(AutoBuildTemplate.DefineClass_Path, FileMode.Create);
            StreamWriter DefineClassFileW = new StreamWriter(DefineClassFileB, System.Text.Encoding.UTF8);
            DefineClassFileW.Write(DefineClassStr);
            DefineClassFileW.Flush();
            DefineClassFileW.Close();
            DefineClassFileB.Close();
            Debug.Log("生成GameDefine代码成功");
            #endregion

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }
    }
}