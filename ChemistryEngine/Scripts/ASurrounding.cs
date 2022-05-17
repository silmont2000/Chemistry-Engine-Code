// 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑
// 作者/Author: silmont@foxmail.com
// 创建时间/Time: 2022.3~2022.5

// ASurrounding.cs

// 一个接收物.
// a receiver
// 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑

using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 接收物类.
/// a receiver
/// </summary>
public class ASurrounding : MonoBehaviour
{
    /// <summary>
    /// 当前接收物拥有的属性列表.
    /// A list of properties owned by the current receiver
    /// </summary>
    public List<AnAttribute> Attributes = new List<AnAttribute>();
    private RuleReadIn RuleManager;
    /// <summary>
    /// 已经激活的属性对应的元素.
    /// The element corresponding to the activated attribute
    /// </summary>
    public int _activeAttribute = -1;
    public int ActiveAttribute
    {
        get { return _activeAttribute; }
        set
        {

            // 如果是设置状态,那么value一定不是-1.如果是-1,说明这个元素即将被消灭,也就没啥好设置的了
            // 接着如果现在还有其他元素在作用,需要先停掉那个元素
            // If it is a setting state, then the value must not be -1. If it is -1, it means that this element is about to be destroyed, and there is nothing to set.
            // Then if there are other elements in action now, you need to stop that element first
            if (value != -1 && _activeAttribute != value && _activeAttribute != -1)
            {
                AnElement ele = gameObject.GetComponent<AnElement>();
                if (ele != null)
                {
                    // 直接删掉这个旧元素,因为后面会增加新的元素脚本上去.
                    // 注意不能经过element_if_active=false,因为那样可能会在协程时删掉新加的元素.
                    // Delete the old element directly, because a new element script will be added later.
                    // Be careful not to pass element_if_active=false, because that may delete the newly added element during the coroutine.
                    ele.Stop();
                }
            }
            _activeAttribute = value;
        }
    }

    /// <summary>
    /// 元素所绑定物体的id,用于活跃元素管理.
    /// The id of the object to which the element is bound, used for active element management.
    /// </summary>
    public int SurroundingInstanceID;

    // /// <summary>
    // /// 元素位置
    // /// </summary>
    // public Transform surrounding_transform;


    /// <summary>
    /// 检查当前元素能否触发属性
    /// Check whether the current element can trigger the attribute
    /// </summary>
    /// <param name="e">当前元素</param>
    /// <returns>能触发返回true,并启动对应属性,否则返回false. Can trigger to return true, and start the corresponding property, otherwise return false</returns>
    public bool CheckRelatedAttributes(AnElement e)
    {
        // Debug.Log(gameObject.name + ",CheckRelatedAttributes " + e.ElementID);
        foreach (AnAttribute attribute in Attributes)
        {
            // Debug.Log(gameObject.name + ",attribute.MatchElement ,CheckRelatedAttributes " + e.ElementID + "," + attribute.MatchElement);
            if (attribute.MatchElement == e.ElementID)
            {
                // Debug.Log(gameObject.name + ",attribute.MatchElement == e.ElementID");
                if (e.ElementIfActive == false)
                {
                    continue;
                }
                if (attribute.Infectiousness)
                {
                    // Debug.Log(gameObject.name);
                    if (gameObject.GetComponent<AnElement>() == null)
                    {
                        gameObject.AddComponent<AnElement>();
                        gameObject.GetComponent<AnElement>().Copy(e);
                    }
                }
                // try
                // {
                    attribute.Active(e, ActiveAttribute);
                // }
                // catch (DelegateException err)
                // {
                //     Debug.LogError($"Chemical Engine: DelegateException: {err.Message}");
                // }
                // Debug.Log(gameObject.name + $"attribute.Active({e.ElementID}, {ActiveAttribute});");
                if (ActiveAttribute == -1 && attribute.IsIsposable)
                {
                    // Debug.Log(gameObject.name + $"Attributes.Remove({attribute.MatchElement});");
                    Attributes.Remove(attribute);
                }
                ActiveAttribute = e.ElementID;
                return true;
            }
        }
        return false;
    }
    private void Start()
    {
        SurroundingInstanceID = gameObject.GetInstanceID();

        // surrounding_transform = gameObject.transform;
    }

    /// <summary>
    /// 寻找元素对应的反应函数句柄.用于检查是否已经绑定或更换绑定的反应回调函数.
    /// Find the reaction function handle corresponding to the element. The reaction callback function used to check whether it has been bound or replaced
    /// </summary>
    /// <param name="match_element">需要寻找的元素</param>
    /// <returns></returns>
    public AnAttribute GetMatchAttribute(int match_element)
    {
        for (int i = 0; i < Attributes.Count; i++)
        {
            if (Attributes[i].MatchElement == match_element)
            {
                return Attributes[i];
            }
        }
        return null;
    }
}
