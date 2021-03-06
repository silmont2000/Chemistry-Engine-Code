// ๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐
// ไฝ่/Author: silmont@foxmail.com
// ๅๅปบๆถ้ด/Time: 2022.3~2022.5

// Environment.cs

// ็ฏๅขๆฐๆฎ.
// Environment data.
// ๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐


using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;


/// <summary>
/// ็ฏๅขๆฐๆฎ.
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
    /// ๆฐๅปบ็ฏๅขๅ้.
    /// New environment variable
    /// </summary>
    /// <param name="list">ๆ็งๅ้ๅ1(string),ๅผ1(float),ๅ้ๅ2,ๅผ2...ๆๅ.Arranged by variable name 1 (string), value 1 (float), variable name 2, value 2...</param>
    public void NewEnbironmentMember(params object[] list)
    {

        // try
        // {
        if (list.Length % 2 != 0)
        {
            throw new LengthException("Chemical Engine: ๅๆฐๆฐ้้ไบ.wrong number of arguments.");
        }

        for (int i = 0; i < list.Length; i = i + 2)
        {
            var name = list[i];
            var value = list[i + 1];
            if (name.GetType() != typeof(string) || value.GetType() != typeof(float))
            {
                throw new TypeException("Chemical Engine: ๅๆฐ็ฑปๅไธๅฏน.ๅชๆฅๅstring-float้ฎๅผๅฏน. The parameter type is wrong. Only string-float key-value pairs are accepted.");
            }
        }
        // }
        // catch (LengthException err)
        // {
        //     Debug.LogError($"Chemical Engine@๊ฐเนยดโข.ฬซโข `เน๊ฑ: LengthException: {err.Message}");

        // }
        // catch (TypeException err)
        // {
        //     Debug.LogError($"Chemical Engine@๊ฐเนยดโข.ฬซโข `เน๊ฑ: TypeException: {err.Message}");

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
            //     Debug.LogError("Chemical Engine@๊ฐเนยดโข.ฬซโข `เน๊ฑ: NewEnbironmentMember: ๅๆฐ็ฑปๅไธๅฏน.ๅชๆฅๅstring-float้ฎๅผๅฏน. The parameter type is wrong. Only string-float key-value pairs are accepted.");
            // }
        }
    }

    void CheckLengthException(object[] list, int j)
    {
        if (list.Length % 2 != 0)
        {
            throw new LengthException("Chemical Engine: ๅๆฐๆฐ้้ไบ.wrong number of arguments.");
        }
    }

    /// <summary>
    /// ่ฎพ็ฝฎ็ฏๅขๅ้
    /// Set the environment data
    /// </summary>
    /// <param name="list">ๆ็งๅ้ๅ1(string),ๅผ1(float),ๅ้ๅ2,ๅผ2...ๆๅ.Arranged by variable name 1 (string), value 1 (float), variable name 2, value 2...</param>
    public void SetEnvironment(params object[] list)
    {

        if (list.Length % 2 != 0)
        {
            throw new LengthException("Chemical Engine: ๅๆฐๆฐ้้ไบ.wrong number of arguments.");
        }

        for (int i = 0; i < list.Length; i = i + 2)
        {
            var name = list[i];
            var value = list[i + 1];
            if (name.GetType() != typeof(string) || value.GetType() != typeof(float))
            {
                throw new TypeException("Chemical Engine: ๅๆฐ็ฑปๅไธๅฏน.ๅชๆฅๅstring-float้ฎๅผๅฏน. The parameter type is wrong. Only string-float key-value pairs are accepted.");
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
                        throw new GetNullException($" Chemical Engine: ็ฏๅขไธญๆฒกๆ{nameStr}. No {nameStr} in the environment.");
                    }
                }
            }
            catch (NullReferenceException e)
            {
                throw new GetNullException($"Chemical Engine: ็ฏๅขไธญๆฒกๆ{nameStr}. No {nameStr} in the environment.");
                // Console.WriteLine("Illegal environmental members!" + e.Message);
            }
        }
    }

    /// <summary>
    /// ่ทๅ็ฏๅขๆฐๆฎ
    /// Get the environment data
    /// </summary>
    /// <param name="name">้่ฆ่ทๅ็็ฏๅขๅ้ๅๅญ.The name of the environment variable to be obtained</param>
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
                    throw new GetNullException($"Chemical Engine: ๆฒกๆๆจๆ่ฏทๆฑ็{nameStr}. no {nameStr} in the environment.");
                }
            }
        }
        catch (NullReferenceException e)
        {
            throw new GetNullException($"Chemical Engine: ๆฒกๆๆจๆ่ฏทๆฑ็{nameStr}. no {nameStr} in the environment.");
        }
        return result;
    }


    /// <summary>
    /// ่ทๅ็ฏๅขๆฐๆฎ
    /// Get the environment data
    /// </summary>
    /// <param name="list">้่ฆ่ทๅ็็ฏๅขๅ้ๅๅญๆฐ็ป.The name of the environment variable to be obtained</param>
    /// <param name="result">้ขๅๅ้ๅฅฝ็็ปๆๆฐ็ป.preallocated result array</param>
    public void GetEnvironment(ref float[] result, params string[] list)
    {

        if (result.Length != list.Length)
        {
            throw new LengthException("Chemical Engine: ็ปๆๆฐ็ปๅคงๅฐไธ่ฏทๆฑๆฐๆฎไธๅน้. The resulting array size does not match the request data.");
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
                        throw new GetNullException($"Chemical Engine: ๆฒกๆๆจๆ่ฏทๆฑ็{nameStr}. no {nameStr} in the environment.");
                    }
                }
            }
            catch (NullReferenceException e)
            {
                throw new GetNullException($"Chemical Engine: ๆฒกๆๆจๆ่ฏทๆฑ็{nameStr}. no {nameStr} in the environment.");
            }
        }
    }
}
