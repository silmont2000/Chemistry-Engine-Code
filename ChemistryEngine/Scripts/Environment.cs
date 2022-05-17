// 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑
// 作者/Author: silmont@foxmail.com
// 创建时间/Time: 2022.3~2022.5

// Environment.cs

// 环境数据.
// Environment data.
// 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑


using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;


/// <summary>
/// 环境数据.
/// Environment data.
/// </summary>
public class Environment
{
    private float wind = 1.0f;
    private float wet = 1.0f;
    private float temprature = 1.0f;
    private Dictionary<string, float> _userEnvironmentMembers = new Dictionary<string, float>();
    private static Environment _environmentInstance;

    public static Environment EnvironmentInstance
    {
        get
        {
            if (_environmentInstance == null)
            {
                _environmentInstance = new Environment();
                // SetElementManger();
            }
            return _environmentInstance;
        }
    }

    /// <summary>
    /// 新建环境变量.
    /// New environment variable
    /// </summary>
    /// <param name="list">按照变量名1(string),值1(float),变量名2,值2...排列.Arranged by variable name 1 (string), value 1 (float), variable name 2, value 2...</param>
    public void NewEnbironmentMember(params object[] list)
    {

        // try
        // {
        if (list.Length % 2 != 0)
        {
            throw new LengthException("Chemical Engine: 参数数量错了.wrong number of arguments.");
        }

        for (int i = 0; i < list.Length; i = i + 2)
        {
            var name = list[i];
            var value = list[i + 1];
            if (name.GetType() != typeof(string) || value.GetType() != typeof(float))
            {
                throw new TypeException("Chemical Engine: 参数类型不对.只接受string-float键值对. The parameter type is wrong. Only string-float key-value pairs are accepted.");
            }
        }
        // }
        // catch (LengthException err)
        // {
        //     Debug.LogError($"Chemical Engine@꒰๑´•.̫• `๑꒱: LengthException: {err.Message}");

        // }
        // catch (TypeException err)
        // {
        //     Debug.LogError($"Chemical Engine@꒰๑´•.̫• `๑꒱: TypeException: {err.Message}");

        // }

        for (int i = 0; i < list.Length; i = i + 2)
        {
            var name = list[i];
            var value = list[i + 1];
            // if (name.GetType() == typeof(string) && value.GetType() == typeof(float))
            // {
            string nameStr = (string)name;
            float valueFloat = (float)value;
            float tmpValue;
            bool hasValue = _userEnvironmentMembers.TryGetValue(nameStr, out tmpValue);
            if (hasValue)
                _userEnvironmentMembers[nameStr] = valueFloat;
            else
                _userEnvironmentMembers.Add(nameStr, valueFloat);
            // }
            // else
            // {
            //     Debug.LogError("Chemical Engine@꒰๑´•.̫• `๑꒱: NewEnbironmentMember: 参数类型不对.只接受string-float键值对. The parameter type is wrong. Only string-float key-value pairs are accepted.");
            // }
        }
    }

    void CheckLengthException(object[] list, int j)
    {
        if (list.Length % 2 != 0)
        {
            throw new LengthException("Chemical Engine: 参数数量错了.wrong number of arguments.");
        }
    }

    /// <summary>
    /// 设置环境变量
    /// Set the environment data
    /// </summary>
    /// <param name="list">按照变量名1(string),值1(float),变量名2,值2...排列.Arranged by variable name 1 (string), value 1 (float), variable name 2, value 2...</param>
    public void SetEnvironment(params object[] list)
    {

        if (list.Length % 2 != 0)
        {
            throw new LengthException("Chemical Engine: 参数数量错了.wrong number of arguments.");
        }

        for (int i = 0; i < list.Length; i = i + 2)
        {
            var name = list[i];
            var value = list[i + 1];
            if (name.GetType() != typeof(string) || value.GetType() != typeof(float))
            {
                throw new TypeException("Chemical Engine: 参数类型不对.只接受string-float键值对. The parameter type is wrong. Only string-float key-value pairs are accepted.");
            }
        }




        for (int i = 0; i < list.Length; i = i + 2)
        {
            var name = list[i];
            var value = list[i + 1];

            string nameStr = (string)name;
            try
            {
                float valueFloat = (float)value;
                // Get Type object of MyClass.
                // Get the FieldInfo by passing the property name and specifying the BindingFlags.
                FieldInfo myPropInfo = this.GetType().GetField(nameStr, BindingFlags.NonPublic | BindingFlags.Instance);
                if (myPropInfo != null)
                    myPropInfo.SetValue(this, valueFloat);
                else
                {
                    float tmpValue;
                    bool hasValue = _userEnvironmentMembers.TryGetValue((string)name, out tmpValue);
                    if (hasValue)
                    {
                        _userEnvironmentMembers[nameStr] = valueFloat;
                    }
                    else
                    {
                        throw new GetNullException($" Chemical Engine: 环境中没有{nameStr}. No {nameStr} in the environment.");
                    }
                }
            }
            catch (NullReferenceException e)
            {
                throw new GetNullException($"Chemical Engine: 环境中没有{nameStr}. No {nameStr} in the environment.");
                // Console.WriteLine("Illegal environmental members!" + e.Message);
            }
        }
    }

    /// <summary>
    /// 获取环境数据
    /// Get the environment data
    /// </summary>
    /// <param name="name">需要获取的环境变量名字.The name of the environment variable to be obtained</param>
    public float GetEnvironment(string name)
    {
        float result = -1;
        string nameStr = (string)name;
        try
        {
            // Get Type object of MyClass.
            // Get the FieldInfo by passing the property name and specifying the BindingFlags.
            FieldInfo myPropInfo = this.GetType().GetField(nameStr, BindingFlags.NonPublic | BindingFlags.Instance);
            if (myPropInfo != null)
                result = (float)myPropInfo.GetValue(this);
            else
            {
                float tmpValue;
                bool hasValue = _userEnvironmentMembers.TryGetValue((string)name, out tmpValue);
                if (hasValue)
                {
                    result = _userEnvironmentMembers[nameStr];
                }
                else
                {
                    throw new GetNullException($"Chemical Engine: 没有您所请求的{nameStr}. no {nameStr} in the environment.");
                }
            }
        }
        catch (NullReferenceException e)
        {
            throw new GetNullException($"Chemical Engine: 没有您所请求的{nameStr}. no {nameStr} in the environment.");
        }
        return result;
    }


    /// <summary>
    /// 获取环境数据
    /// Get the environment data
    /// </summary>
    /// <param name="list">需要获取的环境变量名字数组.The name of the environment variable to be obtained</param>
    /// <param name="result">预先分配好的结果数组.preallocated result array</param>
    public void GetEnvironment(ref float[] result, params string[] list)
    {

        if (result.Length != list.Length)
        {
            throw new LengthException("Chemical Engine: 结果数组大小与请求数据不匹配. The resulting array size does not match the request data.");
        }

        for (int i = 0; i < list.Length; i++)
        {
            var name = list[i];
            string nameStr = (string)name;
            try
            {
                // Get Type object of MyClass.
                // Get the FieldInfo by passing the property name and specifying the BindingFlags.
                FieldInfo myPropInfo = this.GetType().GetField(nameStr, BindingFlags.NonPublic | BindingFlags.Instance);
                if (myPropInfo != null)
                    result[i] = (float)myPropInfo.GetValue(this);
                else
                {
                    float tmpValue;
                    bool hasValue = _userEnvironmentMembers.TryGetValue((string)name, out tmpValue);
                    if (hasValue)
                    {
                        result[i] = _userEnvironmentMembers[nameStr];
                    }
                    else
                    {
                        throw new GetNullException($"Chemical Engine: 没有您所请求的{nameStr}. no {nameStr} in the environment.");
                    }
                }
            }
            catch (NullReferenceException e)
            {
                throw new GetNullException($"Chemical Engine: 没有您所请求的{nameStr}. no {nameStr} in the environment.");
            }
        }
    }
}
