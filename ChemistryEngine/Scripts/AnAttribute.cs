// ððððððððððððððððð
// ä½è/Author: silmont@foxmail.com
// åå»ºæ¶é´/Time: 2022.3~2022.5

// AnAttribute.cs

// ä¸ä¸ªå±æ§,è½å¤å¯¹ç¸åºçåç´ ä½åºååº.
// An attribute that reacts to the corresponding element
// ððððððððððððððððð


using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ä¸ä¸ªå±æ§,è½å¤å¯¹ç¸åºçåç´ ä½åºååº.
/// An attribute that reacts to the corresponding element
/// </summary>
[System.Serializable]
public class AnAttribute
{
    /// <summary>
    /// åªä¸ªåç´ è½è§¦åæ¹å±æ§
    /// </summary>
    public int MatchElement = -2;

    /// <summary>
    /// æ¯å¦ä¸æ¬¡æ§.å¦ææ¯ä¸æ¬¡æ§,é£ä¹å¨è§¦åè¯¥å±æ§æ¶,è¯¥å±æ§ä¼è¢«éæ¯.æ¯å¦è®¾å®æ æ¨åªè½ç¹çä¸æ¬¡ï¼é£ä¹ç¹ççåæ¶æ æ¨ä¼å¤±å»å¯çæ§.
    /// Whether it is one-time. If it is one-time, then when this attribute is triggered, the attribute will be destroyed. For example, if a tree can only be ignited once, then the tree will lose its flammability when ignited.
    /// </summary>
    public bool IsIsposable = true;

    /// <summary>
    /// ä¼ ææ§å¦ä½.trueè¡¨ç¤ºè¿ä¸ªåç´ ä¼è¢«å¤å¶å°æ¥æ¶ç©ä¸,å³"è¢«ä¼ æ".ä¾å¦æ æ¨è¢«ç¹ç,æä¸ºæ°çç«æº,é£ä¹ç«åç´ å°±ä¼è¢«å¤å¶ä¸ä¸ªå°æ æ¨ä¸.
    /// How contagious. true means that this element will be copied to the receiver, that is, "infected". For example, if a tree is ignited and becomes a new source of fire, then the fire element will be copied to the tree.
    /// </summary>
    public bool Infectiousness = false;

    /// <summary>
    /// å£°æå§æ
    /// </summary>
    public delegate void CustomerEventHandler(AnElement e);

    /// <summary>
    /// å½åæ¥æ¶ç©ç¨æ¥è¡¨ç°å±æ§çåè°å½æ°(å§æ).
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
            throw new DelegateException("Chemical Engine: æªç»å®ååºè¡¨ç°å½æ°æåç´ å·²è¢«ç­æ´». Unbound reactive representation function or element has been deactivated.");
        }
        // }
        // catch (DelegateException err)
        // {
        //     Debug.LogError($"Chemical Engine: DelegateException: {err.Message}");

        // }
    }

}