using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wood : RecieverBase
{
    MaterialPropertyBlock props;
    MeshRenderer myRenderer;
    void Start()
    {
        Bound();
        props = new MaterialPropertyBlock();
        myRenderer = gameObject.GetComponent<MeshRenderer>();
        props.SetColor("_Color", new Color(0.7735849f, 0.5086685f, 0.2437522f, 1));
        myRenderer.SetPropertyBlock(props);
    }

    public override void OnFire(AnElement e)
    {
        // Debug.Log(gameObject.name + "OnFire");
        props.SetColor("_Color", new Color(0.8018868f, 0.2753649f, 0.3917273f, 1));
        myRenderer.SetPropertyBlock(props);
        // gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/wood on fire");
    }
}
