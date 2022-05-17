// 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑
// 作者/Author: silmont@foxmail.com
// 创建时间/Time: 2022.3~2022.5

// AnElement.cs

// 一个元素.
// An element
// 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑

using UnityEngine;
using System;
using Object = System.Object;

/// <summary>
/// 一个元素.
/// An element
/// </summary>
[System.Serializable]
public class AnElement : MonoBehaviour
{
    // 距离机制
    // Distance
    // 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑
    #region 
    /// <summary>
    /// 是否有距离触发机制.比如较大的火焰,在物体距离较远,没有发生碰撞时也有可能被点燃,这就是“有距离触发机制”.反之,碰撞时才会发生影响.
    /// Whether there is a distance trigger mechanism. For example, a larger flame may be ignited when the object is far away and there is no collision. This is the "distance trigger mechanism". On the contrary, the impact will only occur when the object collides.
    /// </summary>
    [SerializeField]
    public bool DistanceTrigger;
    /// <summary>
    /// 如果有距离触发机制,作用范围.这个作用范围是在collider基础上向外扩展的范围.
    /// If there is a distance trigger mechanism, the scope of action. This scope of action is the scope that extends outward on the basis of the collider.
    /// </summary>
    public float DistanceRangeOfEffect;
    /// <summary>
    /// 如果有距离触发机制,作用范围的基准计算.勾选表示使用collider最大维度,不勾选代表使用最小维度.
    /// If there is a distance trigger mechanism, the benchmark calculation of the scope of action. Checking means using the maximum dimension of the collider, unchecking means using the minimum dimension
    /// </summary>
    public bool DistanceUseMax;
    /// <summary>
    /// 如果有距离触发机制,作用范围传播时的衰减系数{系数本身也会衰减）.
    /// If there is a distance trigger mechanism, the attenuation coefficient when the range is propagated (the coefficient itself will also be attenuated)
    /// </summary>
    public float DistanceRangeOfEffectAttenuationFactor;
    #endregion

    // 湮灭机制
    // annihilation mechanism
    // 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑

    #region 
    /// <summary>
    /// 是否有湮灭机制.开发者注:复杂场景下不建议使用自带的湮灭机制,因为这无法和您自己的逻辑相贯通,而且衰减是您不可控的.只有在您确保该元素绝对不会与其他元素之间发生转换时才可以使用.
    /// Whether there is an annihilation mechanism. Developer's note: It is not recommended to use the built-in annihilation mechanism in complex scenarios, because it cannot be connected with your own logic, and the attenuation is beyond your control. Only when you ensure that the element is absolutely It can only be used when there is no conversion to and from other elements.
    /// </summary>
    public bool AttenuationTrigger;
    /// <summary>
    /// 如果有湮灭机制,湮灭时是否同时销毁物体.
    /// If there is an annihilation mechanism, whether to destroy the object at the same time when annihilated
    /// </summary>
    public bool AttenuationDestroyGameObject;
    /// <summary>
    /// 如果有湮灭机制,湮灭时间.
    /// If there is an annihilation mechanism, the annihilation time
    /// </summary>
    private Timer AttenuationTimer;
    /// <summary>
    /// 如果有湮灭机制,最开始的作用强度.
    /// If there is an annihilation mechanism, the initial effect strength
    /// </summary>
    public float AttenuationInitialIntensity;
    /// <summary>
    /// 如果有湮灭机制,现在的作用强度.
    /// If there is an annihilation mechanism, the current effect strength
    /// </summary>
    public float AttenuationRemainIntensity;
    /// <summary>
    /// 如果有,作用时间.
    /// If there is, the action time
    /// </summary>
    public int AttenuationSurvivalTime;
    /// <summary>
    /// 如果有,强度衰减方式.0-线性  1-非线性.非线性意味着元素会衰减的越来越快.
    /// If there is, the intensity decay method. 0-linear 1-nonlinear. Nonlinear means that the element will decay faster and faster.
    /// </summary>
    public int AttenuationMode;
    public float _attenuationFactor;
    /// <summary>
    /// 如果衰减方式是非线性,变化比率.
    /// If the decay method is non-linear, the rate of change
    /// </summary>
    public float AttenuationFactor
    {
        get { return _attenuationFactor; }
        set
        {
            _attenuationFactor = value;
            AttenuationSurvivalTime = (int)(-Math.Log10(AttenuationInitialIntensity) / Math.Log10(1 - value));
        }
    }
    // private float _attenuationIntensityAttenuationFactor;
    // public float AttenuationIntensityAttenuationFactor
    // {
    //     get { return _attenuationIntensityAttenuationFactor; }
    //     set
    //     {
    //         _attenuationIntensityAttenuationFactor = value;
    //         AttenuationSurvivalTime = (int)(-Math.Log10(AttenuationInitialIntensity) / Math.Log10(1 - value));
    //     }
    // }
    #endregion


    // 冷冻机制
    // freezing mechanism
    // 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑

    #region 
    /// <summary>
    /// 标记还需要跳过多少帧.(周围是否已经全部是同类元素,如果是,则跳过不需要继续检测）.
    /// Mark how many frames still need to be skipped. {Whether there are all similar elements around, if so, skip and do not need to continue to detect)
    /// </summary>
    [SerializeField]
    public int SkipFrameRemain = 0;
    /// <summary>
    /// 标记下次进入freeze状态需要跳过多少帧.(周围是否已经全部是同类元素,如果是,则跳过不需要继续检测）.
    /// Mark how many frames to skip when entering the freeze state next time. (Whether there are all similar elements around, if so, skip and do not need to continue to detect)
    /// </summary>
    public int SkipFrameNext = 0;
    /// <summary>
    /// 每次skip时冷冻多少帧,要配合道具的移动速度.
    /// How many frames to freeze each time skip, to match the movement speed of the props.
    /// </summary>
    public int SkipFrameSthresh = 0;
    #endregion


    // 元素标记
    // element tag
    // 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑

    #region 
    /// <summary>
    /// 元素名字,以id表示.
    /// Element name, represented by id
    /// </summary>
    public int ElementID;
    /// <summary>
    /// 元素所绑定物体的id,用于活跃元素管理.
    /// The id of the object bound to the element, used for active element management
    /// </summary>
    [SerializeField]
    public int ElementInstanceID;
    #endregion


    // 元素属性
    // element properties
    // 🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑🐑

    #region 
    // /// <summary>
    // /// 元素位置
    // /// </summary>
    // public Transform element_transform;


    // /// <summary>
    // /// 是否会传染给接收物
    // /// </summary>
    // public bool element_if_contagious;


    private bool _elementIfActive;
    /// <summary>
    /// 是否正处于活跃状态,每次设定都会更新一下活跃元素表.
    /// Whether it is in an active state, the active element table will be updated every time it is set.
    /// </summary>
    public bool ElementIfActive
    {
        get { return _elementIfActive; }
        set
        {
            // Debug.Log(gameObject.name + " " + value);
            if (gameObject.GetComponent<Collider>() == null)
            {
                _elementIfActive = false;
                Debug.LogError("Chemical Engine: The element has been destroyed, but you are still trying to activate it. Please check the timing of destroying the element.", transform);
                return;
            }
            _elementIfActive = value;
            ActiveReactionPool.Instance.UpdateList(this);
            // 开启自动湮灭的情况下,激活元素同时启动湮灭计时器.
            // When automatic annihilation is enabled, activate the element and start the annihilation timer at the same time.
            if (value == true)
            {
                CheckSelfAttributes();
            }
            if (value == true && AttenuationTrigger == true)
            {
                AttenuationTiming();
            }
        }
    }
    #endregion


    private void Start()
    {
        ElementInstanceID = gameObject.GetInstanceID();
        // element_transform = gameObject.transform;
    }
    /// <summary>
    /// {开启自动湮灭时）计算剩下的强度.
    /// {When automatic annihilation is enabled) Calculate the remaining strength
    /// </summary>
    /// <param name="value"></param>
    private void CalculateRemainIntensity(Object value)
    {
        if (AttenuationMode == 0)
        {
            AttenuationRemainIntensity = AttenuationTimer.GetRemainTime() / AttenuationSurvivalTime * AttenuationInitialIntensity;
        }
        else
        {
            AttenuationRemainIntensity = (1 - AttenuationFactor) * AttenuationRemainIntensity;
            // AttenuationRemainIntensity = (1 - AttenuationIntensityAttenuationFactor) * AttenuationRemainIntensity;
        }

        if (AttenuationRemainIntensity < 1)
        {
            AttenuationRemainIntensity = 0;
        }
    }
    private void ClockStart()
    {
    }
    /// <summary>
    /// {开启自动湮灭时）开启计时器.
    /// {When automatic annihilation is enabled) start the timer
    /// </summary>
    private void AttenuationTiming()
    {
        AttenuationTimer = new Timer(AttenuationSurvivalTime, true);
        AttenuationRemainIntensity = AttenuationInitialIntensity;
        AttenuationTimer.onEnd = Stop;
        AttenuationTimer.onEnter = ClockStart;
        AttenuationTimer.SetInterval(1f, CalculateRemainIntensity);
    }

    /// <summary>
    /// 符合继续冷冻的条件,则更新冷冻数据.
    /// If the conditions for continued freezing are met, the freezing data is updated
    /// </summary>
    private void AddSkipFrame()
    {
        if (SkipFrameNext < SkipFrameSthresh)
        {
            SkipFrameRemain = SkipFrameNext;
            SkipFrameNext = Math.Min(SkipFrameSthresh, Math.Max(3, SkipFrameNext * 2));
        }
        else
        {
            SkipFrameRemain = SkipFrameNext;
            if (SkipFrameSthresh > 0)
                SkipFrameNext++;
        }
    }

    /// <summary>
    /// 解冻,重置冷冻数据.
    /// Unfreeze, reset frozen data
    /// </summary>
    public void ResetSkipFrame()
    {
        if (SkipFrameSthresh == 0)
            return;
        SkipFrameSthresh = SkipFrameRemain / 2;
        SkipFrameRemain = Math.Min(SkipFrameSthresh, 3);
        // Debug.Log("280 " + this.SkipFrameSthresh);
        // Debug.Log("280 " + this.SkipFrameRemain);

    }


    /// <summary>
    /// 检查当前元素所在物体自己是否能接收自己的元素.
    /// Check whether the object where the current element is located can receive its own element.
    /// </summary>
    public void
    CheckSelfAttributes()
    {
        var tmpSurrounding = gameObject.GetComponent<ASurrounding>();
        if (tmpSurrounding != null)
        {
            tmpSurrounding.CheckRelatedAttributes(this);
        }
    }
    /// <summary>
    /// 检查当前元素作用域内是否有合法接收物,如有,触发属性.
    /// Check whether there is a legal receiver in the scope of the current element, if so, trigger the attribute
    /// </summary>
    public void CheckRelatedSurroundingsOfElement()
    {

        //如果需要继续跳过则不检测
        if (SkipFrameRemain > 0)
        {
            SkipFrameRemain--;
            return;
        }
        // Debug.Log("Checking:" + gameObject.name);
        Collider c = gameObject.GetComponent<Collider>();
        Vector3 pos = this.transform.position;
        float distance_range_of_effect_in_use = 0;
        if (DistanceUseMax)
            distance_range_of_effect_in_use = Math.Max(Math.Max(c.bounds.size.x, c.bounds.size.y), c.bounds.size.z);
        else
            distance_range_of_effect_in_use = Math.Min(Math.Min(c.bounds.size.x, c.bounds.size.y), c.bounds.size.z);
        //如果不是碰撞类型的,更新检测范围
        if (this.DistanceTrigger)
        {
            distance_range_of_effect_in_use += DistanceRangeOfEffect;
        }
        // Debug.Log("    distance_range_of_effect_in_use = " + distance_range_of_effect_in_use);
        int maxColliders = 1024;
        Collider[] hits = new Collider[maxColliders];
        //获取合法接收物列表
        int numColliders = Physics.OverlapSphereNonAlloc(pos, distance_range_of_effect_in_use, hits);
        //只有自己的话,睡眠,跳过
        // Debug.Log("    length: " + numColliders + ", distance_range_of_effect_in_use = " + distance_range_of_effect_in_use);
        if (numColliders <= 1)
        {
            // Debug.Log("    " + gameObject.name + " with " + hits[0].gameObject.name);
            AddSkipFrame();
            return;
        }
        //如果还有别的物体
        else// if (hits.Length > 1)
        {
            bool if_cld = (this.DistanceTrigger == true);
            Collider c_hit = hits[0].GetComponent<Collider>(); ;
            Vector3 hit_pos = c_hit.transform.position;
            float dis1 = 0;
            float dis_a = 0;
            float dis_b = 0;
            float dis_c = 0;
            var tmpSurrounding = c_hit.gameObject.GetComponent<ASurrounding>();
            //影响了多少物体
            int number_of_affected = 0;

            //对这些物体依次计算
            for (int i = 0; i < numColliders; i++)
            {
                // Debug.Log("    " + gameObject.name + " with " + hits[i].gameObject.name);
                tmpSurrounding = hits[i].gameObject.GetComponent<ASurrounding>();

                // 如果是不参与化学计算的物体
                if (tmpSurrounding == null)
                {
                    // Debug.Log("        null");
                    continue;
                }

                if (gameObject.transform == hits[i].gameObject.transform || (tmpSurrounding != null && tmpSurrounding.ActiveAttribute == ElementID))
                {
                    // Debug.Log("        null2");
                    continue;
                }
                //碰撞条件
                // Debug.Log("        get");
                if (!this.DistanceTrigger)
                {
                    c_hit = hits[i].gameObject.GetComponent<Collider>();
                    hit_pos = c_hit.transform.position;
                    dis1 = Vector3.Distance(c.ClosestPointOnBounds(hit_pos), c_hit.ClosestPointOnBounds(pos));
                    dis_a = Vector3.Distance(c.ClosestPointOnBounds(hit_pos), pos);
                    dis_b = Vector3.Distance(hit_pos, c_hit.ClosestPointOnBounds(pos));
                    dis_c = Vector3.Distance(hit_pos, pos);

                    if (dis1 < 1 || dis_a + dis_b >= dis_c)
                    {
                        if_cld = true;
                    }
                }
                //没碰撞,就检查下一个物体
                if (!if_cld) continue;

                //碰撞了,或者本身就是距离型,就进行触发
                if (tmpSurrounding != null && tmpSurrounding.ActiveAttribute != ElementID)
                {
                    // Debug.Log(gameObject.name + " with " + tmpSurrounding.gameObject.name);
                    number_of_affected++;
                    tmpSurrounding.CheckRelatedAttributes(this);
                }
            }
            //一个都没影响,就暂缓检测
            if (number_of_affected == 0)
            {
                AddSkipFrame();
            }
            else
            {
                ResetSkipFrame();
            }
        }

    }

    /// <summary>
    /// 画出球形检测范围
    /// Draw the spherical detection range
    /// </summary>
    private void OnDrawGizmos()
    {
        Collider c = gameObject.GetComponent<Collider>();
        float distance_range_of_effect_in_use = 0;
        if (DistanceUseMax)
            distance_range_of_effect_in_use = Math.Max(Math.Max(c.bounds.size.x, c.bounds.size.y), c.bounds.size.z) / 2;
        else
            distance_range_of_effect_in_use = Math.Min(Math.Min(c.bounds.size.x, c.bounds.size.y), c.bounds.size.z) / 2;

        //如果是碰撞类型的,更新检测范围
        if (this.DistanceTrigger)
        {
            distance_range_of_effect_in_use += DistanceRangeOfEffect;
        }
        // if (distance_trigger)
        Gizmos.DrawWireSphere(this.transform.position, distance_range_of_effect_in_use);
    }


    /// <summary>
    /// 复制元素,通过this.element_if_active = true来同步更新活跃元素表.
    /// Copy the element and update the active element table synchronously by this.element_if_active = true
    /// </summary>
    /// <param name="e"></param>
    public void Copy(AnElement e)
    {

        this.ElementID = e.ElementID;
        this.ElementInstanceID = gameObject.GetInstanceID();

        this.DistanceTrigger = e.DistanceTrigger;
        this.DistanceUseMax = e.DistanceUseMax;
        this.DistanceRangeOfEffectAttenuationFactor = e.DistanceRangeOfEffectAttenuationFactor * e.DistanceRangeOfEffectAttenuationFactor;
        this.DistanceRangeOfEffect = e.DistanceRangeOfEffect * e.DistanceRangeOfEffectAttenuationFactor;
        if (this.DistanceRangeOfEffect < 0.3)
        {
            this.DistanceTrigger = false;
        }

        this.SkipFrameRemain = Math.Min(UnityEngine.Random.Range(3, 60), e.SkipFrameSthresh);
        // Debug.Log("copy" + this.SkipFrameRemain);
        this.SkipFrameNext = this.SkipFrameRemain;
        this.SkipFrameSthresh = e.SkipFrameSthresh;

        this.AttenuationTrigger = e.AttenuationTrigger;
        this.AttenuationMode = e.AttenuationMode;
        this.AttenuationInitialIntensity = e.AttenuationInitialIntensity * e.DistanceRangeOfEffectAttenuationFactor;
        this.AttenuationRemainIntensity = this.AttenuationInitialIntensity;
        // this.AttenuationIntensityAttenuationFactor = e.AttenuationIntensityAttenuationFactor;
        this.AttenuationFactor = e.AttenuationFactor;
        if (this.AttenuationMode == 0)
        {
            this.AttenuationSurvivalTime = (int)(e.AttenuationSurvivalTime * e.DistanceRangeOfEffectAttenuationFactor);
        }
        this.AttenuationDestroyGameObject = e.AttenuationDestroyGameObject;
        //这一步和计时器初始化有关,需要放在attenuation_trigger赋值之后.这之前已经删除旧元素从元素池,现在改成新的,再加入元素池
        this.ElementIfActive = true;
    }

    /// <summary>
    /// 销毁当前元素
    /// Destroy the current element
    /// </summary>
    public void Stop()
    {
        // element_if_active = false;
        ASurrounding srd = gameObject.GetComponent<ASurrounding>();
        if (srd != null && srd.ActiveAttribute == ElementID)
        {
            srd.ActiveAttribute = -1;
        }
        if (AttenuationDestroyGameObject)
            // 删除挂载着脚本的游戏物体
            Destroy(gameObject);
        else
        {
            // 移除脚本自身
            Destroy(this);
        }
    }

}
