#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace Games
{
	public partial class GameTag 
    {


        [MenuItem("GameObject/MyTool/SetEditorTag")]
        public static void SetEditorTag ()
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            while (it.NextVisible(true))
            {
                if(it.name == "tags")
                {

					it.ClearArray();
					int end = customBeginIndex + customTags.Length;
					if(it.arraySize < end)
					{
						it.arraySize = end;
					}

                    for (int i = customBeginIndex; i < end; i++) 
                    {
                        SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);
                        dataPoint.stringValue = customTags[i - customBeginIndex];
                    }

                    tagManager.ApplyModifiedProperties();
                    
                    break;
                }
            }
        }

		[MenuItem("GameObject/MyTool/Generate GameTag.cs")]
		public static void GenerateGameTag ()
		{
			SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
			SerializedProperty it = tagManager.GetIterator();
			while (it.NextVisible(true))
			{
				if(it.name == "tags")
				{
					StringWriter sw = new StringWriter();
					sw.WriteLine("namespace Games");
					sw.WriteLine("{");
					sw.WriteLine("\tpublic partial class GameTag");
					sw.WriteLine("\t{");

					sw.WriteLine("\t\t#region Unity Default Lock");
					sw.WriteLine("\t\tpublic const string Untagged        = \"Untagged\";");
					sw.WriteLine("\t\tpublic const string Respawn         = \"Respawn\";");
					sw.WriteLine("\t\tpublic const string Finish          = \"Finish\";");
					sw.WriteLine("\t\tpublic const string EditorOnly      = \"EditorOnly\";");
					sw.WriteLine("\t\tpublic const string MainCamera      = \"MainCamera\";");
					sw.WriteLine("\t\tpublic const string Player          = \"Player\";");
					sw.WriteLine("\t\tpublic const string GameController  = \"GameController\";");

					sw.WriteLine("\t\t#endregion");
					sw.WriteLine("\n");


					string fieldnameStr = "";
					string gap = "";

					for (int i = 0; i < it.arraySize; i++) 
					{
						SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);
						string fieldname = string.IsNullOrEmpty(dataPoint.stringValue) ? "Tag" + i : dataPoint.stringValue.Replace(" ", "_");

						sw.WriteLine(string.Format("\t\tpublic const string {0}  = \"{1}\";", fieldname, dataPoint.stringValue));


						fieldnameStr += gap;
						fieldnameStr += string.Format("\"{0}\"", dataPoint.stringValue);
						gap = ",";
					}


					sw.WriteLine("\n");
					sw.WriteLine("\t\tpublic static int           customBeginIndex = 0;");
					sw.WriteLine("\t\tpublic static string[]      customTags = {"+fieldnameStr+"};");

					sw.WriteLine("\t}");
					sw.WriteLine("}");
					File.WriteAllText(Application.dataPath + "/ChemistryEngineScripts@silmont/Editor/GenLayer/GameTag.cs", sw.ToString(), System.Text.Encoding.UTF8);
					AssetDatabase.Refresh();

					break;
				}
			}
		}
    }
}

#endif