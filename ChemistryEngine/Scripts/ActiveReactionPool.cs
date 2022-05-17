// 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑
// 作者/Author: silmont@foxmail.com
// 创建时间/Time: 2022.3~2022.5

// ActiveReactionPool.cs

// 用于活跃元素池管理.
// For active element pool management
// 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 用于活跃元素池管理.
/// For active element pool management
/// </summary>

[DisallowMultipleComponent]
public class ActiveReactionPool : MonoBehaviour
{
    /// <summary>
    /// 目前在元素池中活跃的元素.意味着在下一轮会对这些元素进行监测.
    /// Elements currently active in the element pool. Means that these elements will be monitored in the next round
    /// </summary>
    private List<AnElement> _activeElements = new List<AnElement>();
    /// <summary>
    /// 目前在元素池中活跃的而且运动非常快的元素.意味着不会等到下一轮，而是每一帧都监测.
    /// Elements that are currently active in the element pool and are moving very fast. Means not waiting for the next round, but monitoring every frame.
    /// </summary>
    private List<AnElement> _activeHighSpeedElements = new List<AnElement>();
    /// <summary>
    /// 目前预备被销毁的元素.意味着在下一轮监测前会先被剔除和销毁.
    /// The element that is currently ready to be destroyed. It means that it will be eliminated and destroyed before the next round of monitoring
    /// </summary>
    private List<AnElement> _deadElements = new List<AnElement>();
    /// <summary>
    /// 标志本轮监测是否已经结束.
    /// Indicates whether the current round of monitoring has ended
    /// </summary>
    private bool _isIEnumerableCheckReactionFinish = true;
    /// <summary>
    /// 元素种类管理器.
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
    /// 规则管理器.
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
    /// 元素池单例.
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
    /// 首先剔除应当销毁的元素,接着对元素池中的所有活跃元素进行监测.这种监测会持续n帧,以保证性能.
    /// First remove the elements that should be destroyed, and then monitor all active elements in the element pool. This monitoring will continue for several frames to ensure performance
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerableCheckReaction()
    {
        // 标志本轮监测仍未结束.
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
            // 以防万一被手动删了
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
        // 标志本轮监测结束.
        // This marks the end of this round of monitoring
        _isIEnumerableCheckReactionFinish = true;
        yield return null;
    }
    // void Start()
    // {
    // }

    /// <summary>
    /// 一轮又一轮地开启协程.
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
            // 以防万一被手动删了
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
    /// 新元素加入时,根据状态对元素池进行增删改查.
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
            // 以防这时候用户已经把物体给删了
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