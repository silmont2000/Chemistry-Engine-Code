using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : RecieverBase
{
    float _per = 0.0f;
    bool _onFireFlag = false;
    // bool _fireFlag = false;
    int _elementName = -1;

    void Start()
    {
        Bound();
        _elementName = gameObject.GetComponent<AnElement>().ElementID;
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
        Debug.Log("bound");
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(gameObject.name + gameObject.GetComponent<Renderer>().material.GetFloat("_DissolvePercentage"));
        if (_onFireFlag)
        {
            gameObject.GetComponent<Renderer>().material.SetFloat("_DissolvePercentage", _per);
            _per += 0.001f;
            // Debug.Log(gameObject.name + ",add");
            if (_per >= 1.0)
            {
                _onFireFlag = false;
                gameObject.GetComponent<AnElement>().ElementIfActive = false;
                Destroy(gameObject);
            }
        }
        if (Input.GetKey("p"))
        {
            //  gameObject.GetComponent<AnElement>().ElementIfActive = true;
        }
    }

    public override void OnFire(AnElement e)
    {
        _onFireFlag = true;
        // gameObject.GetComponent<Renderer>().material.SetFloat("_DissolvePercentage", per);
        // Debug.Log(gameObject.name + "OnFire");
    }
    private void OnTriggerEnter(Collider other)
    {

        // Debug.Log(gameObject.name + ".OnTriggerEnter with " + other.gameObject.name);
        // if (_fireFlag)
        //     return;
        ASurrounding tmp;
        if ((tmp = other.gameObject.GetComponent<ASurrounding>()) != null && tmp.GetMatchAttribute(_elementName) != null)
        {
            if (!gameObject.GetComponent<AnElement>().ElementIfActive)
            {
                gameObject.GetComponent<AnElement>().ElementIfActive = true;
                gameObject.GetComponent<SphereCollider>().isTrigger = false;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
