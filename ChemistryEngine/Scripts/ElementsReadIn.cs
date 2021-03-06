// ๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐
// ไฝ่/Author: silmont@foxmail.com
// ๅๅปบๆถ้ด/Time: 2022.3~2022.5

// ElementsReadIn.cs

// ่ด่ดฃ่ฏปjsonๆไปถไธญ็ๅ็ด ๆฐๆฎ.
// Responsible for reading element data in json file.
// ๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐

using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEditor;
/// <summary>
/// ไปฅ้ฒไนๅไผๆทปๅ ๆฐ็้็ฝฎๅฑๆง,ๅไปฅ็ปๆไฝ็ๆนๅผๅญๅจ.
/// In case new configuration properties will be added later, store them as structures first
/// </summary>
public struct AnElementName
{
    public string EleType { get; set; }
}

/// <summary>
/// ็ฎก็jsonไธญๅทฒๆ็ๆๆๅ็ด ็ง็ฑป
/// ่ด่ดฃ่ฏปjsonๆไปถไธญ็ๅ็ด ๆฐๆฎ.
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
    /// ่ทๅๆๆๅ็ด ็ๅๅญ{UI๏ผ
    /// Get the names of all elements {UI)
    /// </summary>
    /// <returns>ๅๅญ็stringๆฐ็ป</returns>
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
    /// ่ทๅๅ็ด ๅๅญๆฐ้
    /// Get the number of element names
    /// </summary>
    /// <returns>ๆฐ้int</returns>
    public int GetElementLength()
    {
        // SetElementManger();
        return _names.Length;
    }
    /// <summary>
    /// ่ฏปๅjsonไธญ็ๆๆๅ็ด 
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
            throw new ReadInFileException($"Chemical Engine: ๆฃๆฅๆไปถ่ทฏๅพ:{path}. Please check current path:{path}");
        }
        StreamReader streamreader = new StreamReader(path); // ่ฏปๅๆฐๆฎ,่ฝฌๆขๆๆฐๆฎๆต
        JsonReader js = new JsonReader(streamreader);   // ๅ่ฝฌๆขๆjsonๆฐๆฎ
        _names = JsonMapper.ToObject<AnElementName[]>(js);
        // Debug.Log("get " + _names.Length);
        streamreader.Close();
        streamreader.Dispose();
        // Debug.Log($"Chemistry Engine: ๆไปถ:{streamreader}");
        // Debug.Log($"Chemistry Engine: ๆไปถ่ทฏๅพ:{path}");
        Debug.Log($"Chemistry Engine: Element Manager init, get {_names.Length} elemnets.");

    }

    private static string GetPath(string _scriptName)
    {
        string[] path = UnityEditor.AssetDatabase.FindAssets(_scriptName);
        if (path.Length > 1)
        {
            // try
            // {
            throw new ReadInFileException($"Chemical Engine: ่ทฏๅพ้ไบๆๆๅๅๆไปถ.ๅฝๅ่ทฏๅพๆฏ{path}. The path is wrong or there is a file with the same name. The current path is {path}");
            // }
            // catch (ReadInFileException err)
            // {
            //     Debug.LogError($"Chemical Engine: ReadInFileException: {err.Message}");
            // }
        }
        //ๅฐๅญ็ฌฆไธฒไธญๅพ่ๆฌๅๅญๅๅ็ผ็ป็ปๅป้คๆ
        string _path = AssetDatabase.GUIDToAssetPath(path[0]).Replace((@"/" + _scriptName + ".cs"), "");
        return _path;
    }
}