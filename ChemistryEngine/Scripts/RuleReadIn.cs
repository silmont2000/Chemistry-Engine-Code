using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.IO;
using System.Text;

using LitJson;

/// <summary>
/// 规则类.
/// Rule
/// </summary>
public struct ARule
{
    public string FuncName { get; set; }
    public string PrevElementID { get; set; }
    public string NextElementID { get; set; }
}


/// <summary>
/// 规则管理器
/// Rule Manager
/// </summary>
public class RuleReadIn
{
    public static ARule[] _rules;
    private static RuleReadIn _ruleManager;

    public static RuleReadIn RuleManagerInstance
    {
        get
        {
            if (_ruleManager == null)
            {
                _ruleManager = new RuleReadIn();
            }
            return _ruleManager;
        }
    }
    public RuleReadIn()
    {
        SetRuleManger();
    }
    /// <summary>
    /// 读取json中的所有规则
    /// Read all rules in json
    /// </summary>
    private static void SetRuleManger()
    {
        string path = "";
        // try
        // {
            path = GetPath("RuleReadIn") + "\\StreamingAssets\\Config\\RuleManager.json";
        // }
        // catch (ReadInFileException err)
        // {
        //     Debug.LogError($"Chemical Engine@꒰๑´•.̫• `๑꒱: ReadInFileException: {err.Message}");

        // }
        // if (!File.Exists(path))
        // {
        //     return;
        // }
        StreamReader streamreader = new StreamReader(path); // 读取数据,转换成数据流
        JsonReader js = new JsonReader(streamreader);   // 再转换成json数据
        _rules = JsonMapper.ToObject<ARule[]>(js);
        streamreader.Close();
        streamreader.Dispose();
        CreateScripts(GetPath("RuleReadIn"), "RecieverBase");
    }


    private static string GetPath(string _scriptName)
    {
        string[] path = UnityEditor.AssetDatabase.FindAssets(_scriptName);
        if (path.Length > 1)
        {
            throw new ReadInFileException($"Chemical Engine: 路径错了或有同名文件.当前路径是{path}. The path is wrong or there is a file with the same name. The current path is {path}");
            // Debug.LogError($"Chemical Engine@꒰๑´•.̫• `๑꒱: ElementsReadIn: 路径错了或有同名文件.当前路径是{path}. The path is wrong or there is a file with the same name. The current path is {path}");
            // return null;
        }
        //将字符串中得脚本名字和后缀统统去除掉
        string _path = AssetDatabase.GUIDToAssetPath(path[0]).Replace((@"/" + _scriptName + ".cs"), "");
        return _path;
    }

    [MenuItem("GameObject/MyTool/CreateScripts", priority = 0)]
    private static void CreateScripts()
    {
        foreach (Object item in Selection.objects)
        {
            var className = item.name;
            var path = Application.dataPath + "/ChemistryEngineScripts@silmont/Scenes";
            CreateScripts(path, className);
        }
    }

    private static void CreateScripts(string path, string className)
    {
        string _path = path;
        string _className = className;
        string _classNameDis = className + ".cs";
        string _newPath = _path + "/" + _classNameDis;

        FileInfo fileInfo = new FileInfo(_path + ".meta");

        if (!fileInfo.Exists)
        {
            throw new System.Exception(string.Format($"Chemical Engine: 文件夹路径不存在：{_path}"));
        }

        // //简单的重名处理
        // if (File.Exists(_newPath))
        // {
        //     _className = _className + "-" + UnityEngine.Random.Range(0, 100) + "." + "cs";
        //     _newPath = _path + "/" + _className;
        // }

        File.WriteAllText(_newPath, ReturnName(_className));
        AssetDatabase.Refresh();
        Debug.Log($"Chemistry Engine: Rule Manager init, create RecieverBase at {_path}.");

    }

    private static string ReturnName(string className)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(
            "using UnityEngine;" +
            "" +
            $"public class {className} : MonoBehaviour" +
            "{" +
            "    ASurrounding this_surrounding;" +
            "    public void Bound()" +
            "    {" +
            "        this_surrounding = gameObject.GetComponent<ASurrounding>();" +
            "        AnAttribute tmp;"
        );

        foreach (ARule rule in _rules)
        {
            // Debug.Log($"{rule.NextElementID},{rule.PrevElementID}, {rule.FuncName}");
            sb.AppendLine(
                $"        if ((tmp = this_surrounding.GetMatchAttribute({rule.NextElementID})) != null)" +
                $"            tmp.SetHandler({rule.PrevElementID}, {rule.FuncName});"
            );
        }


        sb.AppendLine("    }");
        foreach (ARule rule in _rules)
        {
            sb.AppendLine(
            $"    public virtual void {rule.FuncName}(AnElement e)" +
            "    {}"
            );
        }
        sb.AppendLine("}");

        return sb.ToString();
    }
}