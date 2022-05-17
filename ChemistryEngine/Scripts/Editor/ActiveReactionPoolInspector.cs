using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System;

[DisallowMultipleComponent]
[CustomEditor(typeof(ActiveReactionPool))] //指定要编辑的脚本对象
/// <summary>
/// AnElement的面板UI
/// </summary>
public class ActiveReactionPoolInspector : Editor
{
    private ActiveReactionPool _activeReactionPool;
    private void OnEnable()
    {
        _activeReactionPool = (ActiveReactionPool)target;
    }
    public override void OnInspectorGUI()
    {
        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        if (GUILayout.Button("更新接收物脚本\n Update RecieverBase Script"))
        {
            _activeReactionPool.SetElementAndRule();
        }
    }


}
