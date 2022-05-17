#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System;
using UnityEditor;
using System.IO;

namespace Games
{
	public class GameSortingLayerEditor
	{
		public static bool IsUseBitIndex = true;

		[MenuItem("GameObject/MyTool/SetEditorSortingLayer")]
		public static void SetEditorTag ()
		{
			SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
			SerializedProperty it = tagManager.GetIterator();
			while (it.NextVisible(true))
			{
				if(it.name == "m_SortingLayers")
				{
					int[] 		ids 	;
					string[] 	names 	;

					EnumUtil.GetValuesAndFieldNames<GameSortingLayer>(out ids, out names);
					it.ClearArray();

					int length = ids.Length;
					if(it.arraySize < length)
					{
						it.arraySize = length;
					}


					for (int i = 0; i < length; i++) 
					{
						SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);
						SerializedProperty namePoint = dataPoint.FindPropertyRelative("name");
						SerializedProperty uniqueIDPoint = dataPoint.FindPropertyRelative("uniqueID");
						namePoint.stringValue = names[i];
						uniqueIDPoint.intValue = IsUseBitIndex ? 1 << (ids[i] - 1) : ids[i];
					}

					tagManager.ApplyModifiedProperties();
					break;
				}
			}
		}

		[MenuItem("GameObject/MyTool/Generate GameSortingLayer.cs")]
		public static void GameSortingLayer ()
		{
			SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
			SerializedProperty it = tagManager.GetIterator();
			while (it.NextVisible(true))
			{
				if(it.name == "m_SortingLayers")
				{
					StringWriter sw = new StringWriter();
					sw.WriteLine("namespace Games");
					sw.WriteLine("{");
					sw.WriteLine("\tpublic enum GameSortingLayer");
					sw.WriteLine("\t{");

					int bitIndex = 0;
					for (int i = 0; i < it.arraySize; i++) 
					{
						SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);
						SerializedProperty namePoint = dataPoint.FindPropertyRelative("name");
						SerializedProperty uniqueIDPoint = dataPoint.FindPropertyRelative("uniqueID");
						Debug.Log(namePoint.stringValue + " = " + uniqueIDPoint.intValue);
						if(uniqueIDPoint.intValue == 0)
						{
							bitIndex = i;
							break;
						}
					}

					Debug.Log(bitIndex);

					for (int i = 0; i < it.arraySize; i++) 
					{
						SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);
						SerializedProperty namePoint = dataPoint.FindPropertyRelative("name");
						SerializedProperty uniqueIDPoint = dataPoint.FindPropertyRelative("uniqueID");
						sw.WriteLine(string.Format("\t\t{0}\t\t\t=\t{1},", namePoint.stringValue.Replace(" ", "_"), IsUseBitIndex ? i - bitIndex : uniqueIDPoint.intValue ));
					}

					sw.WriteLine("\t}");
					sw.WriteLine("}");


					File.WriteAllText(Application.dataPath + "/ChemistryEngineScripts@silmont/Editor/GenLayer/GameSortingLayer.cs", sw.ToString(), System.Text.Encoding.UTF8);
					AssetDatabase.Refresh();
					break;
				}
			}
		}
	}

}

#endif