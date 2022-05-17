// 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑
// 作者/Author: silmont@foxmail.com
// 创建时间/Time: 2022.3~2022.5

// ElementsReadIn.cs

// 负责读json文件中的元素数据.
// Responsible for reading element data in json file.
// 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑

using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEditor;
/// <summary>
/// 以防之后会添加新的配置属性,先以结构体的方式存储.
/// In case new configuration properties will be added later, store them as structures first
/// </summary>
public struct AnElementName
{
    public string EleType { get; set; }
}

/// <summary>
/// 管理json中已有的所有元素种类
/// 负责读json文件中的元素数据.
/// Responsible for reading element data in json file.
/// </summary>
public class ElementsReadIn
{
    private static AnElementName[] _names;
    private static ElementsReadIn _elementManager = new ElementsReadIn();

    public static ElementsReadIn ElementManagerInstance
    {
        get
        {
            if (_elementManager == null)
            {
                _elementManager = new ElementsReadIn();
                // SetElementManger();
            }
            return _elementManager;
        }
    }

    public ElementsReadIn()
    {
        // Debug.Log("hello1");
        SetElementManger();
    }
    /// <summary>
    /// 获取所有元素的名字{UI）
    /// Get the names of all elements {UI)
    /// </summary>
    /// <returns>名字的string数组</returns>
    public string[] GetElementNames()
    {
        // Debug.Log("hello2");
        // SetElementManger();
        List<string> readinNames = new List<string>();
        foreach (AnElementName aname in _names)
        {
            readinNames.Add(aname.EleType);
        }
        return readinNames.ToArray();
    }
    /// <summary>
    /// 获取元素名字数量
    /// Get the number of element names
    /// </summary>
    /// <returns>数量int</returns>
    public int GetElementLength()
    {
        // SetElementManger();
        return _names.Length;
    }
    /// <summary>
    /// 读取json中的所有元素
    /// Read all elements in json
    /// </summary>
    private static void SetElementManger()
    {
        string path = "";
        // try
        // {
        path = GetPath("ElementsReadIn") + "\\StreamingAssets\\Config\\ElementManager.json";
        // }
        // catch (ReadInFileException err)
        // {
        //     Debug.LogError($"Chemical Engine: ReadInFileException: {err.Message}");
        // }


        if (!File.Exists(path))
        {
            throw new ReadInFileException($"Chemical Engine: 检查文件路径:{path}. Please check current path:{path}");
        }
        StreamReader streamreader = new StreamReader(path); // 读取数据,转换成数据流
        JsonReader js = new JsonReader(streamreader);   // 再转换成json数据
        _names = JsonMapper.ToObject<AnElementName[]>(js);
        // Debug.Log("get " + _names.Length);
        streamreader.Close();
        streamreader.Dispose();
        // Debug.Log($"Chemistry Engine: 文件:{streamreader}");
        // Debug.Log($"Chemistry Engine: 文件路径:{path}");
        Debug.Log($"Chemistry Engine: Element Manager init, get {_names.Length} elemnets.");

    }

    private static string GetPath(string _scriptName)
    {
        string[] path = UnityEditor.AssetDatabase.FindAssets(_scriptName);
        if (path.Length > 1)
        {
            // try
            // {
            throw new ReadInFileException($"Chemical Engine: 路径错了或有同名文件.当前路径是{path}. The path is wrong or there is a file with the same name. The current path is {path}");
            // }
            // catch (ReadInFileException err)
            // {
            //     Debug.LogError($"Chemical Engine: ReadInFileException: {err.Message}");
            // }
        }
        //将字符串中得脚本名字和后缀统统去除掉
        string _path = AssetDatabase.GUIDToAssetPath(path[0]).Replace((@"/" + _scriptName + ".cs"), "");
        return _path;
    }
}