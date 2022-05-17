// 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑
// 作者/Author: silmont@foxmail.com
// 创建时间/Time: 2022.3~2022.5

// AnAttribute.cs

// 一个属性,能够对相应的元素作出反应.
// An attribute that reacts to the corresponding element
// 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑


using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个属性,能够对相应的元素作出反应.
/// An attribute that reacts to the corresponding element
/// </summary>
[System.Serializable]
public class AnAttribute
{
    /// <summary>
    /// 哪个元素能触发改属性
    /// </summary>
    public int MatchElement = -2;

    /// <summary>
    /// 是否一次性.如果是一次性,那么在触发该属性时,该属性会被销毁.比如设定树木只能点燃一次，那么点燃的同时树木会失去可燃性.
    /// Whether it is one-time. If it is one-time, then when this attribute is triggered, the attribute will be destroyed. For example, if a tree can only be ignited once, then the tree will lose its flammability when ignited.
    /// </summary>
    public bool IsIsposable = true;

    /// <summary>
    /// 传染性如何.true表示这个元素会被复制到接收物上,即"被传染".例如树木被点燃,成为新的火源,那么火元素就会被复制一个到树木上.
    /// How contagious. true means that this element will be copied to the receiver, that is, "infected". For example, if a tree is ignited and becomes a new source of fire, then the fire element will be copied to the tree.
    /// </summary>
    public bool Infectiousness = false;

    /// <summary>
    /// 声明委托
    /// </summary>
    public delegate void CustomerEventHandler(AnElement e);

    /// <summary>
    /// 当前接收物用来表现属性的回调函数(委托).
    /// The callback function (delegate) used by the current receiver to represent the property
    /// </summary>
    // public CustomerEventHandler Handler;
    public Dictionary<int, CustomerEventHandler> Handlers = new Dictionary<int, CustomerEventHandler>();

    public void SetHandler(int prev_e, CustomerEventHandler h)
    {
        CustomerEventHandler tmpValue;
        bool hasValue = Handlers.TryGetValue(prev_e, out tmpValue);
        if (hasValue)
            Handlers[prev_e] += h;
        else
        {
            Handlers.Add(prev_e, h);
        }
    }
    private Environment _environment;
    public void Active(AnElement e, int prev_e = -1)
    {
        if (prev_e == e.ElementID)
            return;
        CustomerEventHandler tmpValue;
        // try
        // {
        bool hasValue = Handlers.TryGetValue(prev_e, out tmpValue);
        if (hasValue && e.ElementIfActive == true)
        {
            Handlers[prev_e](e);
        }
        else
        {
            throw new DelegateException("Chemical Engine: 未绑定反应表现函数或元素已被灭活. Unbound reactive representation function or element has been deactivated.");
        }
        // }
        // catch (DelegateException err)
        // {
        //     Debug.LogError($"Chemical Engine: DelegateException: {err.Message}");

        // }
    }

}