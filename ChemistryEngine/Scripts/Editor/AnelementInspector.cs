#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System;

[CanEditMultipleObjects]
[CustomEditor(typeof(AnElement))] //指定要编辑的脚本对象
/// <summary>
/// AnElement的面板UI
/// </summary>
public class AnElementPropInspector : Editor
{
    private AnElement _anElement;

    private ElementsReadIn ElementManagerInstance;

    private void OnEnable()
    {
        _anElement = (AnElement)target;
        ElementManagerInstance = ActiveReactionPool.ElementManager;
    }

    /// <summary>
    /// 选择的元素index
    /// </summary>
    // static private int selected_element_index = 1;
    private float range_of_effect;
    private string[] available_attenuation_mode = { "线性递减", "非线性递减" };
    public override void OnInspectorGUI()
    {

        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

        var tmp = -Math.Log10(_anElement.AttenuationRemainIntensity) / Math.Log10(1 - _anElement.AttenuationFactor);
        EditorGUILayout.LabelField("===========运行数据 Runtime Data===========", style, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        EditorGUILayout.LabelField($"当前还会冷冻{_anElement.SkipFrameRemain}帧.It will continue to freeze for {_anElement.SkipFrameRemain} frames now", style, GUILayout.ExpandWidth(true));
        if (_anElement.AttenuationTrigger)
        {
            // EditorGUILayout.LabelField("运行时数据:");
            EditorGUILayout.LabelField($"当前强度{_anElement.AttenuationRemainIntensity},还能活{tmp}秒.The current strength is {_anElement.AttenuationRemainIntensity}, and can live for another {tmp} seconds", style, GUILayout.ExpandWidth(true));
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();


        EditorGUILayout.LabelField("===========元素属性 Element Attribute===========", style, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        _anElement.ElementID = EditorGUILayout.Popup("元素类型.Type(ID)", _anElement.ElementID, ElementManagerInstance.GetElementNames());
        _anElement.DistanceRangeOfEffectAttenuationFactor = EditorGUILayout.Slider("衰减系数.DistanceRangeOfEffectAttenuationFactor", _anElement.DistanceRangeOfEffectAttenuationFactor, 0.001f, 1);
        EditorGUILayout.Space();
        EditorGUILayout.Space();


        EditorGUILayout.LabelField("===========触发机制 Trigger Mechanism===========", style, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        _anElement.DistanceTrigger = EditorGUILayout.Toggle("靠近作用.DistanceTrigger", _anElement.DistanceTrigger);
        _anElement.DistanceUseMax = EditorGUILayout.Toggle("使用最大维度基准.UseMaxDimension", _anElement.DistanceUseMax);
        if (_anElement.DistanceTrigger)
        {
            _anElement.DistanceRangeOfEffect = EditorGUILayout.Slider("作用范围.RangeOfEffect", _anElement.DistanceRangeOfEffect, 0, 500);
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();


        EditorGUILayout.LabelField("===========湮灭机制 Attenuation Mechanism===========", style, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        _anElement.AttenuationTrigger = EditorGUILayout.Toggle("自动湮灭.AttenuationTrigger", _anElement.AttenuationTrigger);
        if (_anElement.AttenuationTrigger)
        {
            _anElement.AttenuationDestroyGameObject = EditorGUILayout.Toggle("湮灭后毁灭游戏对象.DestroyGameObject", _anElement.AttenuationDestroyGameObject);
            _anElement.AttenuationInitialIntensity = EditorGUILayout.FloatField("初始强度.InitialIntensity", _anElement.AttenuationInitialIntensity);
            _anElement.AttenuationMode = EditorGUILayout.Popup("强度衰减方式.AttenuationMode", _anElement.AttenuationMode, available_attenuation_mode);
            if (_anElement.AttenuationMode == 0)
            {
                _anElement.AttenuationSurvivalTime = EditorGUILayout.IntField("生存时间(s).SurvivalTime", _anElement.AttenuationSurvivalTime);
                if (_anElement.AttenuationSurvivalTime <= 1)
                {
                    EditorGUILayout.LabelField("生存周期需>1.SurvivalTime must >1");
                    _anElement.AttenuationSurvivalTime = 1;
                }
            }
            else if (_anElement.AttenuationMode == 1)
            {
                _anElement.AttenuationFactor = (float)EditorGUILayout.Slider("非线性衰减系数.AttenuationFactor", _anElement.AttenuationFactor, 0.001f, 1);
                tmp = -Math.Log10(_anElement.AttenuationInitialIntensity) / Math.Log10(1 - _anElement.AttenuationFactor);
                _anElement.AttenuationSurvivalTime = (int)tmp;
                EditorGUILayout.LabelField($"注意,强度衰减为1时会自动湮灭.预计生存时间{_anElement.AttenuationSurvivalTime}秒.Note that when the intensity decays to 1, it will automatically annihilate. Expected survival time {_anElement.AttenuationSurvivalTime} seconds");
            }
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();


        EditorGUILayout.LabelField("===========性能 Performance===========", style, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        _anElement.SkipFrameSthresh = (int)EditorGUILayout.Slider("无接收物时的冷冻帧数.SkipFrameSthresh", _anElement.SkipFrameSthresh, 0, 1000); //参数{标签名,滑动值,最小值,最大值）
        serializedObject.ApplyModifiedProperties();
    }


}

#endif