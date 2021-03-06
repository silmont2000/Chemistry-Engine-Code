// ๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐
// ไฝ่/Author: silmont@foxmail.com
// ๅๅปบๆถ้ด/Time: 2022.3~2022.5

// ASurrounding.cs

// ไธไธชๆฅๆถ็ฉ.
// a receiver
// ๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐

using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ๆฅๆถ็ฉ็ฑป.
/// a receiver
/// </summary>
public class ASurrounding : MonoBehaviour
{
    /// <summary>
    /// ๅฝๅๆฅๆถ็ฉๆฅๆ็ๅฑๆงๅ่กจ.
    /// A list of properties owned by the current receiver
    /// </summary>
    public List<AnAttribute> Attributes = new List<AnAttribute>();
    private RuleReadIn RuleManager;
    /// <summary>
    /// ๅทฒ็ปๆฟๆดป็ๅฑๆงๅฏนๅบ็ๅ็ด .
    /// The element corresponding to the activated attribute
    /// </summary>
    public int _activeAttribute = -1;
    public int ActiveAttribute
    {
        get { return _activeAttribute; }
        set
        {

            // ๅฆๆๆฏ่ฎพ็ฝฎ็ถๆ,้ฃไนvalueไธๅฎไธๆฏ-1.ๅฆๆๆฏ-1,่ฏดๆ่ฟไธชๅ็ด ๅณๅฐ่ขซๆถ็ญ,ไนๅฐฑๆฒกๅฅๅฅฝ่ฎพ็ฝฎ็ไบ
            // ๆฅ็ๅฆๆ็ฐๅจ่ฟๆๅถไปๅ็ด ๅจไฝ็จ,้่ฆๅๅๆ้ฃไธชๅ็ด 
            // If it is a setting state, then the value must not be -1. If it is -1, it means that this element is about to be destroyed, and there is nothing to set.
            // Then if there are other elements in action now, you need to stop that element first
            if (value != -1 && _activeAttribute != value && _activeAttribute != -1)
            {
                AnElement ele = gameObject.GetComponent<AnElement>();
                if (ele != null)
                {
                    // ็ดๆฅๅ ๆ่ฟไธชๆงๅ็ด ,ๅ ไธบๅ้ขไผๅขๅ ๆฐ็ๅ็ด ่ๆฌไธๅป.
                    // ๆณจๆไธ่ฝ็ป่ฟelement_if_active=false,ๅ ไธบ้ฃๆ ทๅฏ่ฝไผๅจๅ็จๆถๅ ๆๆฐๅ ็ๅ็ด .
                    // Delete the old element directly, because a new element script will be added later.
                    // Be careful not to pass element_if_active=false, because that may delete the newly added element during the coroutine.
                    ele.Stop();
                }
            }
            _activeAttribute = value;
        }
    }

    /// <summary>
    /// ๅ็ด ๆ็ปๅฎ็ฉไฝ็id,็จไบๆดป่ทๅ็ด ็ฎก็.
    /// The id of the object to which the element is bound, used for active element management.
    /// </summary>
    public int SurroundingInstanceID;

    // /// <summary>
    // /// ๅ็ด ไฝ็ฝฎ
    // /// </summary>
    // public Transform surrounding_transform;


    /// <summary>
    /// ๆฃๆฅๅฝๅๅ็ด ่ฝๅฆ่งฆๅๅฑๆง
    /// Check whether the current element can trigger the attribute
    /// </summary>
    /// <param name="e">ๅฝๅๅ็ด </param>
    /// <returns>่ฝ่งฆๅ่ฟๅtrue,ๅนถๅฏๅจๅฏนๅบๅฑๆง,ๅฆๅ่ฟๅfalse. Can trigger to return true, and start the corresponding property, otherwise return false</returns>
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
    /// ๅฏปๆพๅ็ด ๅฏนๅบ็ๅๅบๅฝๆฐๅฅๆ.็จไบๆฃๆฅๆฏๅฆๅทฒ็ป็ปๅฎๆๆดๆข็ปๅฎ็ๅๅบๅ่ฐๅฝๆฐ.
    /// Find the reaction function handle corresponding to the element. The reaction callback function used to check whether it has been bound or replaced
    /// </summary>
    /// <param name="match_element">้่ฆๅฏปๆพ็ๅ็ด </param>
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
