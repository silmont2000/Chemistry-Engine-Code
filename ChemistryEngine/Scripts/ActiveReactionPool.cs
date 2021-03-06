// đđđđđđđđđđđđđđđđđ
// äŊč/Author: silmont@foxmail.com
// ååģēæļé´/Time: 2022.3~2022.5

// ActiveReactionPool.cs

// į¨äēæ´ģčˇåį´ æą įŽĄį.
// For active element pool management
// đđđđđđđđđđđđđđđđđ

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// į¨äēæ´ģčˇåį´ æą įŽĄį.
/// For active element pool management
/// </summary>

[DisallowMultipleComponent]
public class ActiveReactionPool : MonoBehaviour
{
    /// <summary>
    /// įŽåå¨åį´ æą ä¸­æ´ģčˇįåį´ .æåŗįå¨ä¸ä¸čŊŽäŧå¯ščŋäēåį´ čŋčĄįæĩ.
    /// Elements currently active in the element pool. Means that these elements will be monitored in the next round
    /// </summary>
    private List<AnElement> _activeElements = new List<AnElement>();
    /// <summary>
    /// įŽåå¨åį´ æą ä¸­æ´ģčˇįčä¸čŋå¨éå¸¸åŋĢįåį´ .æåŗįä¸äŧį­å°ä¸ä¸čŊŽīŧčæ¯æ¯ä¸å¸§éŊįæĩ.
    /// Elements that are currently active in the element pool and are moving very fast. Means not waiting for the next round, but monitoring every frame.
    /// </summary>
    private List<AnElement> _activeHighSpeedElements = new List<AnElement>();
    /// <summary>
    /// įŽåéĸå¤čĸĢéæ¯įåį´ .æåŗįå¨ä¸ä¸čŊŽįæĩåäŧåčĸĢåé¤åéæ¯.
    /// The element that is currently ready to be destroyed. It means that it will be eliminated and destroyed before the next round of monitoring
    /// </summary>
    private List<AnElement> _deadElements = new List<AnElement>();
    /// <summary>
    /// æ åŋæŦčŊŽįæĩæ¯åĻåˇ˛įģįģæ.
    /// Indicates whether the current round of monitoring has ended
    /// </summary>
    private bool _isIEnumerableCheckReactionFinish = true;
    /// <summary>
    /// åį´ į§įąģįŽĄįå¨.
    /// Element Kind Manager
    /// </summary>
    public static ElementsReadIn ElementManager;
    // {
    //     get
    //     {
    //         return ElementsReadIn.ElementManagerInstance;
    //     }
    // }
    /// <summary>
    /// č§åįŽĄįå¨.
    /// Rules Manager
    /// </summary>
    public static RuleReadIn RuleManager;
    // {
    //     get
    //     {
    //         return RuleReadIn.RuleManagerInstance;
    //     }
    // }
    /// <summary>
    /// åį´ æą åäž.
    /// Element Pool Singleton
    /// </summary>
    private static ActiveReactionPool _instance;
    public static ActiveReactionPool Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ActiveReactionPool();
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        SetElementAndRule();
    }


    [SerializeField]
    public void SetElementAndRule()
    {
        ElementManager = new ElementsReadIn();
        RuleManager = new RuleReadIn();
    }

    /// <summary>
    /// éĻååé¤åēåŊéæ¯įåį´ ,æĨįå¯šåį´ æą ä¸­įæææ´ģčˇåį´ čŋčĄįæĩ.čŋį§įæĩäŧæįģ­nå¸§,äģĨäŋč¯æ§čŊ.
    /// First remove the elements that should be destroyed, and then monitor all active elements in the element pool. This monitoring will continue for several frames to ensure performance
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerableCheckReaction()
    {
        // æ åŋæŦčŊŽįæĩäģæĒįģæ.
        // Signs that this round of monitoring is not over yet
        _isIEnumerableCheckReactionFinish = false;
        DeleteActiveElement();
        yield return null;
        var length = _activeElements.Count;
        // var step = (int)Math.Sqrt(length);
        // var step = (int)Math.Sqrt(length);
        // var step = 30;
        for (int j = 0; j < length; j++)
        {
            // äģĨé˛ä¸ä¸čĸĢæå¨å äē
            if (_activeElements[j] != null)
            {
                // Debug.Log("next check " + _activeElements[j].gameObject.name);
                _activeElements[j].CheckRelatedSurroundingsOfElement();
            }
            else
            {
                RemoveActiveElement(_activeElements[j]);
                continue;
            }
            // if (j % step == 0)
            // {
            //     yield return null;
            // }
        }
        // æ åŋæŦčŊŽįæĩįģæ.
        // This marks the end of this round of monitoring
        _isIEnumerableCheckReactionFinish = true;
        yield return null;
    }
    // void Start()
    // {
    // }

    /// <summary>
    /// ä¸čŊŽåä¸čŊŽå°åŧå¯åį¨.
    /// Start the coroutine round by round
    /// </summary>
    void Update()
    {
        var length = _activeHighSpeedElements.Count;
        for (int j = 0; j < length; j++)
        {
            if (j >= _activeHighSpeedElements.Count)
                break;
            // Debug.Log(j + "," + _activeHighSpeedElements.Count);
            // äģĨé˛ä¸ä¸čĸĢæå¨å äē
            if (_activeHighSpeedElements[j] != null)
            {
                _activeHighSpeedElements[j].CheckRelatedSurroundingsOfElement();
            }
            else
            {
                RemoveActiveElement(_activeHighSpeedElements[j]);
                continue;
            }
        }

        if (_isIEnumerableCheckReactionFinish)
            StartCoroutine("IEnumerableCheckReaction");
    }

    /// <summary>
    /// æ°åį´ å åĨæļ,æ šæŽįļæå¯šåį´ æą čŋčĄåĸå æšæĨ.
    /// When a new element is added, the element pool is added, deleted, modified and checked according to the status
    /// </summary>
    /// <param name="element"></param>
    public void UpdateList(AnElement element)
    {
        // Debug.Log("update " + element.ElementIfActive);
        if (element.ElementIfActive == true)
        {
            AddActiveElement(element);
        }
        else
        {
            RemoveActiveElement(element);
        }
        // Debug.Log("count " + _activeElements.Count);
    }
    void AddActiveElement(AnElement element)
    {
        if (element.SkipFrameSthresh == 0)
            _activeHighSpeedElements.Add(element);
        else
            _activeElements.Add(element);
    }

    void RemoveActiveElement(AnElement element)
    {
        if (element.SkipFrameSthresh == 0)
        {
            Predicate<AnElement> matches = (AnElement e) => { return e.ElementInstanceID == element.ElementInstanceID; };
            _activeHighSpeedElements.RemoveAll(matches);
        }
        else
            _deadElements.Add(element);
    }

    void DeleteActiveElement()
    {
        var length = _deadElements.Count;
        Predicate<AnElement> matches = (AnElement e) => { return e.ElementInstanceID == _deadElements[0].ElementInstanceID; };
        for (int j = 0; j < length; j++)
        {
            // äģĨé˛čŋæļåį¨æˇåˇ˛įģæįŠäŊįģå äē
            if (_deadElements[j] != null)
                _deadElements[j].Stop();
            matches = (AnElement e) => { return e.ElementInstanceID == _deadElements[j].ElementInstanceID; };
            _activeElements.RemoveAll(matches);
            // _activeHighSpeedElements.RemoveAll(matches);
        }
        _deadElements.Clear();
    }

    public int GetCoroutineActiveElementLength()
    {
        return _activeElements.Count;
    }
    public int GetUpdateActiveElementLength()
    {
        return _activeHighSpeedElements.Count;
    }

}