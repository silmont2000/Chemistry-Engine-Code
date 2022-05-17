#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;


namespace Games
{
	public partial class GameLayer
    {


        [MenuItem("GameObject/MyTool/SetEditorLayer")]
        public static void SetEditorTag ()
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            while (it.NextVisible(true))
            {
                if(it.name == "layers")
                {
                    int end = Mathf.Min(customBeginIndex + customLayers.Length, it.arraySize);
                    for (int i = customBeginIndex; i < end; i++) 
                    {
                        SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);
                        dataPoint.stringValue = customLayers[i - customBeginIndex];
                    }

                    tagManager.ApplyModifiedProperties();
                    if (customBeginIndex + customLayers.Length > 32)
                    {
						Debug.LogFormat("<color=red>Layer不能超过31</color>");
                    }
                    break;
                }
            }
        }

		[MenuItem("GameObject/MyTool/Generate GameLayer.cs")]
		public static void GenerateGameLayer ()
		{
			SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
			SerializedProperty it = tagManager.GetIterator();
			while (it.NextVisible(true))
			{
				if(it.name == "layers")
				{
					StringWriter sw = new StringWriter();
					sw.WriteLine("namespace Games");
					sw.WriteLine("{");
					sw.WriteLine("\tpublic partial class GameLayer");
					sw.WriteLine("\t{");

					sw.WriteLine("\t\t#region Unity Default Lock");
					for (int i = 0; i < 6; i++) 
					{
						SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);
						string fieldname = string.IsNullOrEmpty(dataPoint.stringValue) ? "Layer" + i : dataPoint.stringValue.Replace(" ", "_");
						sw.WriteLine(string.Format("\t\tpublic const int {0}\t\t\t=\t1 << {1};", fieldname , i ));

					}
					sw.WriteLine("\t\t#endregion");
					sw.WriteLine("\n");

					List<string> fieldnames = new List<string>();
					for (int i = 6; i < it.arraySize; i++) 
					{
						SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);
						string fieldname = string.IsNullOrEmpty(dataPoint.stringValue) ? "Layer" + i : dataPoint.stringValue.Replace(" ", "_");
						sw.WriteLine(string.Format("\t\tpublic const int {0}\t\t\t=\t1 << {1};", fieldname , i ));
						fieldnames.Add(string.Format("\"{0}\"", fieldname));
					}

					string fieldnameStr = "";
					string gap = "";
					for(int i = 0; i < fieldnames.Count; i ++)
					{
						fieldnameStr += gap;
						fieldnameStr += fieldnames[i];
						gap = ",";
					}


					sw.WriteLine("\n");
					sw.WriteLine("\t\tpublic static int           customBeginIndex = 6;");
					sw.WriteLine("\t\tpublic static string[]      customLayers = {"+fieldnameStr+"};");

					sw.WriteLine("\t}");
					sw.WriteLine("}");
					File.WriteAllText(Application.dataPath + "/ChemistryEngineScripts@silmont/Editor/GenLayer/GameLayer.cs", sw.ToString(), System.Text.Encoding.UTF8);
					AssetDatabase.Refresh();
					break;
				}
			}
		}
    }
}

#endif