// πππππππππππππππππ
// δ½θ/Author: silmont@foxmail.com
// εε»ΊζΆι΄/Time: 2022.3~2022.5
// NoBoundException.cs
// πππππππππππππππππ
using System;

/// <summary>
/// θ°η¨εεΊε½ζ°εΌεΈΈ
/// </summary>
public class DelegateException : ApplicationException
{
    public DelegateException(string message) : base(message)
    {
    }
}

/// <summary>
/// θ°η¨ε·²η­ζ΄»εη΄ εΌεΈΈ
/// </summary>
public class ActivateExtinguishedElementException : ApplicationException
{
    public ActivateExtinguishedElementException(string message) : base(message)
    {
    }
}

/// <summary>
/// ζ°η»ιΏεΊ¦δΈεΉιεΌεΈΈ
/// </summary>
public class LengthException : ApplicationException
{
    public LengthException(string message) : base(message)
    {
    }
}

/// <summary>
/// εζ°η±»εδΈεΉιεΌεΈΈ
/// </summary>
public class TypeException : ApplicationException
{
    public TypeException(string message) : base(message)
    {
    }
}

/// <summary>
/// θ―»ζδ»ΆεΌεΈΈ
/// </summary>
public class ReadInFileException : ApplicationException
{
    public ReadInFileException(string message) : base(message)
    {
    }
}

/// <summary>
/// η©ΊθΏεεΌεΈΈ
/// </summary>
public class GetNullException : ApplicationException
{
    public GetNullException(string message) : base(message)
    {
    }
}

