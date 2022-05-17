// 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑
// 作者/Author: silmont@foxmail.com
// 创建时间/Time: 2022.3~2022.5
// NoBoundException.cs
// 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑
using System;

/// <summary>
/// 调用反应函数异常
/// </summary>
public class DelegateException : ApplicationException
{
    public DelegateException(string message) : base(message)
    {
    }
}

/// <summary>
/// 调用已灭活元素异常
/// </summary>
public class ActivateExtinguishedElementException : ApplicationException
{
    public ActivateExtinguishedElementException(string message) : base(message)
    {
    }
}

/// <summary>
/// 数组长度不匹配异常
/// </summary>
public class LengthException : ApplicationException
{
    public LengthException(string message) : base(message)
    {
    }
}

/// <summary>
/// 参数类型不匹配异常
/// </summary>
public class TypeException : ApplicationException
{
    public TypeException(string message) : base(message)
    {
    }
}

/// <summary>
/// 读文件异常
/// </summary>
public class ReadInFileException : ApplicationException
{
    public ReadInFileException(string message) : base(message)
    {
    }
}

/// <summary>
/// 空返回异常
/// </summary>
public class GetNullException : ApplicationException
{
    public GetNullException(string message) : base(message)
    {
    }
}

