using UnityEngine;public class RecieverBase : MonoBehaviour{    ASurrounding this_surrounding;    public void Bound()    {        this_surrounding = gameObject.GetComponent<ASurrounding>();        AnAttribute tmp;
        if ((tmp = this_surrounding.GetMatchAttribute(0)) != null)            tmp.SetHandler(-1, OnFire);
        if ((tmp = this_surrounding.GetMatchAttribute(1)) != null)            tmp.SetHandler(-1, OnIce);
        if ((tmp = this_surrounding.GetMatchAttribute(2)) != null)            tmp.SetHandler(-1, OnWater);
        if ((tmp = this_surrounding.GetMatchAttribute(0)) != null)            tmp.SetHandler(1, OnIceFire);
        if ((tmp = this_surrounding.GetMatchAttribute(0)) != null)            tmp.SetHandler(2, OnWaterFire);
        if ((tmp = this_surrounding.GetMatchAttribute(1)) != null)            tmp.SetHandler(0, OnFireIce);
        if ((tmp = this_surrounding.GetMatchAttribute(1)) != null)            tmp.SetHandler(2, OnWaterIce);
        if ((tmp = this_surrounding.GetMatchAttribute(2)) != null)            tmp.SetHandler(0, OnFireWater);
        if ((tmp = this_surrounding.GetMatchAttribute(2)) != null)            tmp.SetHandler(1, OnIceWater);
    }
    public virtual void OnFire(AnElement e)    {}
    public virtual void OnIce(AnElement e)    {}
    public virtual void OnWater(AnElement e)    {}
    public virtual void OnIceFire(AnElement e)    {}
    public virtual void OnWaterFire(AnElement e)    {}
    public virtual void OnFireIce(AnElement e)    {}
    public virtual void OnWaterIce(AnElement e)    {}
    public virtual void OnFireWater(AnElement e)    {}
    public virtual void OnIceWater(AnElement e)    {}
}
