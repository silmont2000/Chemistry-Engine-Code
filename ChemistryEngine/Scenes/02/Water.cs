using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    float _per = 0.0f;
    bool _onFireFlag = false;
    // bool _fireFlag = false;
    int _elementName = -1;

    void Start()
    {
        // Bound();
        _elementName = gameObject.GetComponent<AnElement>().ElementID;
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.name + " trigger with" + other.gameObject.name);
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
