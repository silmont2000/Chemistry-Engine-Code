// ๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐
// ไฝ่/Author: silmont@foxmail.com
// ๅๅปบๆถ้ด/Time: 2022.3~2022.5

// AnElement.cs

// ไธไธชๅ็ด .
// An element
// ๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐

using UnityEngine;
using System;
using Object = System.Object;

/// <summary>
/// ไธไธชๅ็ด .
/// An element
/// </summary>
[System.Serializable]
public class AnElement : MonoBehaviour
{
    // ่ท็ฆปๆบๅถ
    // Distance
    // ๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐
    #region 
    /// <summary>
    /// ๆฏๅฆๆ่ท็ฆป่งฆๅๆบๅถ.ๆฏๅฆ่พๅคง็็ซ็ฐ,ๅจ็ฉไฝ่ท็ฆป่พ่ฟ,ๆฒกๆๅ็็ขฐๆๆถไนๆๅฏ่ฝ่ขซ็น็,่ฟๅฐฑๆฏโๆ่ท็ฆป่งฆๅๆบๅถโ.ๅไน,็ขฐๆๆถๆไผๅ็ๅฝฑๅ.
    /// Whether there is a distance trigger mechanism. For example, a larger flame may be ignited when the object is far away and there is no collision. This is the "distance trigger mechanism". On the contrary, the impact will only occur when the object collides.
    /// </summary>
    [SerializeField]
    public bool DistanceTrigger;
    /// <summary>
    /// ๅฆๆๆ่ท็ฆป่งฆๅๆบๅถ,ไฝ็จ่ๅด.่ฟไธชไฝ็จ่ๅดๆฏๅจcolliderๅบ็กไธๅๅคๆฉๅฑ็่ๅด.
    /// If there is a distance trigger mechanism, the scope of action. This scope of action is the scope that extends outward on the basis of the collider.
    /// </summary>
    public float DistanceRangeOfEffect;
    /// <summary>
    /// ๅฆๆๆ่ท็ฆป่งฆๅๆบๅถ,ไฝ็จ่ๅด็ๅบๅ่ฎก็ฎ.ๅพ้่กจ็คบไฝฟ็จcolliderๆๅคง็ปดๅบฆ,ไธๅพ้ไปฃ่กจไฝฟ็จๆๅฐ็ปดๅบฆ.
    /// If there is a distance trigger mechanism, the benchmark calculation of the scope of action. Checking means using the maximum dimension of the collider, unchecking means using the minimum dimension
    /// </summary>
    public bool DistanceUseMax;
    /// <summary>
    /// ๅฆๆๆ่ท็ฆป่งฆๅๆบๅถ,ไฝ็จ่ๅดไผ ๆญๆถ็่กฐๅ็ณปๆฐ{็ณปๆฐๆฌ่บซไนไผ่กฐๅ๏ผ.
    /// If there is a distance trigger mechanism, the attenuation coefficient when the range is propagated (the coefficient itself will also be attenuated)
    /// </summary>
    public float DistanceRangeOfEffectAttenuationFactor;
    #endregion

    // ๆนฎ็ญๆบๅถ
    // annihilation mechanism
    // ๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐

    #region 
    /// <summary>
    /// ๆฏๅฆๆๆนฎ็ญๆบๅถ.ๅผๅ่ๆณจ:ๅคๆๅบๆฏไธไธๅปบ่ฎฎไฝฟ็จ่ชๅธฆ็ๆนฎ็ญๆบๅถ,ๅ ไธบ่ฟๆ ๆณๅๆจ่ชๅทฑ็้ป่พ็ธ่ดฏ้,่ไธ่กฐๅๆฏๆจไธๅฏๆง็.ๅชๆๅจๆจ็กฎไฟ่ฏฅๅ็ด ็ปๅฏนไธไผไธๅถไปๅ็ด ไน้ดๅ็่ฝฌๆขๆถๆๅฏไปฅไฝฟ็จ.
    /// Whether there is an annihilation mechanism. Developer's note: It is not recommended to use the built-in annihilation mechanism in complex scenarios, because it cannot be connected with your own logic, and the attenuation is beyond your control. Only when you ensure that the element is absolutely It can only be used when there is no conversion to and from other elements.
    /// </summary>
    public bool AttenuationTrigger;
    /// <summary>
    /// ๅฆๆๆๆนฎ็ญๆบๅถ,ๆนฎ็ญๆถๆฏๅฆๅๆถ้ๆฏ็ฉไฝ.
    /// If there is an annihilation mechanism, whether to destroy the object at the same time when annihilated
    /// </summary>
    public bool AttenuationDestroyGameObject;
    /// <summary>
    /// ๅฆๆๆๆนฎ็ญๆบๅถ,ๆนฎ็ญๆถ้ด.
    /// If there is an annihilation mechanism, the annihilation time
    /// </summary>
    private Timer AttenuationTimer;
    /// <summary>
    /// ๅฆๆๆๆนฎ็ญๆบๅถ,ๆๅผๅง็ไฝ็จๅผบๅบฆ.
    /// If there is an annihilation mechanism, the initial effect strength
    /// </summary>
    public float AttenuationInitialIntensity;
    /// <summary>
    /// ๅฆๆๆๆนฎ็ญๆบๅถ,็ฐๅจ็ไฝ็จๅผบๅบฆ.
    /// If there is an annihilation mechanism, the current effect strength
    /// </summary>
    public float AttenuationRemainIntensity;
    /// <summary>
    /// ๅฆๆๆ,ไฝ็จๆถ้ด.
    /// If there is, the action time
    /// </summary>
    public int AttenuationSurvivalTime;
    /// <summary>
    /// ๅฆๆๆ,ๅผบๅบฆ่กฐๅๆนๅผ.0-็บฟๆง  1-้็บฟๆง.้็บฟๆงๆๅณ็ๅ็ด ไผ่กฐๅ็่ถๆฅ่ถๅฟซ.
    /// If there is, the intensity decay method. 0-linear 1-nonlinear. Nonlinear means that the element will decay faster and faster.
    /// </summary>
    public int AttenuationMode;
    public float _attenuationFactor;
    /// <summary>
    /// ๅฆๆ่กฐๅๆนๅผๆฏ้็บฟๆง,ๅๅๆฏ็.
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


    // ๅทๅปๆบๅถ
    // freezing mechanism
    // ๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐

    #region 
    /// <summary>
    /// ๆ ่ฎฐ่ฟ้่ฆ่ทณ่ฟๅคๅฐๅธง.(ๅจๅดๆฏๅฆๅทฒ็ปๅจ้จๆฏๅ็ฑปๅ็ด ,ๅฆๆๆฏ,ๅ่ทณ่ฟไธ้่ฆ็ปง็ปญๆฃๆต๏ผ.
    /// Mark how many frames still need to be skipped. {Whether there are all similar elements around, if so, skip and do not need to continue to detect)
    /// </summary>
    [SerializeField]
    public int SkipFrameRemain = 0;
    /// <summary>
    /// ๆ ่ฎฐไธๆฌก่ฟๅฅfreeze็ถๆ้่ฆ่ทณ่ฟๅคๅฐๅธง.(ๅจๅดๆฏๅฆๅทฒ็ปๅจ้จๆฏๅ็ฑปๅ็ด ,ๅฆๆๆฏ,ๅ่ทณ่ฟไธ้่ฆ็ปง็ปญๆฃๆต๏ผ.
    /// Mark how many frames to skip when entering the freeze state next time. (Whether there are all similar elements around, if so, skip and do not need to continue to detect)
    /// </summary>
    public int SkipFrameNext = 0;
    /// <summary>
    /// ๆฏๆฌกskipๆถๅทๅปๅคๅฐๅธง,่ฆ้ๅ้ๅท็็งปๅจ้ๅบฆ.
    /// How many frames to freeze each time skip, to match the movement speed of the props.
    /// </summary>
    public int SkipFrameSthresh = 0;
    #endregion


    // ๅ็ด ๆ ่ฎฐ
    // element tag
    // ๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐

    #region 
    /// <summary>
    /// ๅ็ด ๅๅญ,ไปฅid่กจ็คบ.
    /// Element name, represented by id
    /// </summary>
    public int ElementID;
    /// <summary>
    /// ๅ็ด ๆ็ปๅฎ็ฉไฝ็id,็จไบๆดป่ทๅ็ด ็ฎก็.
    /// The id of the object bound to the element, used for active element management
    /// </summary>
    [SerializeField]
    public int ElementInstanceID;
    #endregion


    // ๅ็ด ๅฑๆง
    // element properties
    // ๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐

    #region 
    // /// <summary>
    // /// ๅ็ด ไฝ็ฝฎ
    // /// </summary>
    // public Transform element_transform;


    // /// <summary>
    // /// ๆฏๅฆไผไผ ๆ็ปๆฅๆถ็ฉ
    // /// </summary>
    // public bool element_if_contagious;


    private bool _elementIfActive;
    /// <summary>
    /// ๆฏๅฆๆญฃๅคไบๆดป่ท็ถๆ,ๆฏๆฌก่ฎพๅฎ้ฝไผๆดๆฐไธไธๆดป่ทๅ็ด ่กจ.
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
            // ๅผๅฏ่ชๅจๆนฎ็ญ็ๆๅตไธ,ๆฟๆดปๅ็ด ๅๆถๅฏๅจๆนฎ็ญ่ฎกๆถๅจ.
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
    /// {ๅผๅฏ่ชๅจๆนฎ็ญๆถ๏ผ่ฎก็ฎๅฉไธ็ๅผบๅบฆ.
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
    /// {ๅผๅฏ่ชๅจๆนฎ็ญๆถ๏ผๅผๅฏ่ฎกๆถๅจ.
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
    /// ็ฌฆๅ็ปง็ปญๅทๅป็ๆกไปถ,ๅๆดๆฐๅทๅปๆฐๆฎ.
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
    /// ่งฃๅป,้็ฝฎๅทๅปๆฐๆฎ.
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
    /// ๆฃๆฅๅฝๅๅ็ด ๆๅจ็ฉไฝ่ชๅทฑๆฏๅฆ่ฝๆฅๆถ่ชๅทฑ็ๅ็ด .
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
    /// ๆฃๆฅๅฝๅๅ็ด ไฝ็จๅๅๆฏๅฆๆๅๆณๆฅๆถ็ฉ,ๅฆๆ,่งฆๅๅฑๆง.
    /// Check whether there is a legal receiver in the scope of the current element, if so, trigger the attribute
    /// </summary>
    public void CheckRelatedSurroundingsOfElement()
    {

        //ๅฆๆ้่ฆ็ปง็ปญ่ทณ่ฟๅไธๆฃๆต
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
        //ๅฆๆไธๆฏ็ขฐๆ็ฑปๅ็,ๆดๆฐๆฃๆต่ๅด
        if (this.DistanceTrigger)
        {
            distance_range_of_effect_in_use += DistanceRangeOfEffect;
        }
        // Debug.Log("    distance_range_of_effect_in_use = " + distance_range_of_effect_in_use);
        int maxColliders = 1024;
        Collider[] hits = new Collider[maxColliders];
        //่ทๅๅๆณๆฅๆถ็ฉๅ่กจ
        int numColliders = Physics.OverlapSphereNonAlloc(pos, distance_range_of_effect_in_use, hits);
        //ๅชๆ่ชๅทฑ็่ฏ,็ก็ ,่ทณ่ฟ
        // Debug.Log("    length: " + numColliders + ", distance_range_of_effect_in_use = " + distance_range_of_effect_in_use);
        if (numColliders <= 1)
        {
            // Debug.Log("    " + gameObject.name + " with " + hits[0].gameObject.name);
            AddSkipFrame();
            return;
        }
        //ๅฆๆ่ฟๆๅซ็็ฉไฝ
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
            //ๅฝฑๅไบๅคๅฐ็ฉไฝ
            int number_of_affected = 0;

            //ๅฏน่ฟไบ็ฉไฝไพๆฌก่ฎก็ฎ
            for (int i = 0; i < numColliders; i++)
            {
                // Debug.Log("    " + gameObject.name + " with " + hits[i].gameObject.name);
                tmpSurrounding = hits[i].gameObject.GetComponent<ASurrounding>();

                // ๅฆๆๆฏไธๅไธๅๅญฆ่ฎก็ฎ็็ฉไฝ
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
                //็ขฐๆๆกไปถ
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
                //ๆฒก็ขฐๆ,ๅฐฑๆฃๆฅไธไธไธช็ฉไฝ
                if (!if_cld) continue;

                //็ขฐๆไบ,ๆ่ๆฌ่บซๅฐฑๆฏ่ท็ฆปๅ,ๅฐฑ่ฟ่ก่งฆๅ
                if (tmpSurrounding != null && tmpSurrounding.ActiveAttribute != ElementID)
                {
                    // Debug.Log(gameObject.name + " with " + tmpSurrounding.gameObject.name);
                    number_of_affected++;
                    tmpSurrounding.CheckRelatedAttributes(this);
                }
            }
            //ไธไธช้ฝๆฒกๅฝฑๅ,ๅฐฑๆ็ผๆฃๆต
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
    /// ็ปๅบ็ๅฝขๆฃๆต่ๅด
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

        //ๅฆๆๆฏ็ขฐๆ็ฑปๅ็,ๆดๆฐๆฃๆต่ๅด
        if (this.DistanceTrigger)
        {
            distance_range_of_effect_in_use += DistanceRangeOfEffect;
        }
        // if (distance_trigger)
        Gizmos.DrawWireSphere(this.transform.position, distance_range_of_effect_in_use);
    }


    /// <summary>
    /// ๅคๅถๅ็ด ,้่ฟthis.element_if_active = trueๆฅๅๆญฅๆดๆฐๆดป่ทๅ็ด ่กจ.
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
        //่ฟไธๆญฅๅ่ฎกๆถๅจๅๅงๅๆๅณ,้่ฆๆพๅจattenuation_trigger่ตๅผไนๅ.่ฟไนๅๅทฒ็ปๅ ้คๆงๅ็ด ไปๅ็ด ๆฑ ,็ฐๅจๆนๆๆฐ็,ๅๅ ๅฅๅ็ด ๆฑ 
        this.ElementIfActive = true;
    }

    /// <summary>
    /// ้ๆฏๅฝๅๅ็ด 
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
            // ๅ ้คๆ่ฝฝ็่ๆฌ็ๆธธๆ็ฉไฝ
            Destroy(gameObject);
        else
        {
            // ็งป้ค่ๆฌ่ช่บซ
            Destroy(this);
        }
    }

}
