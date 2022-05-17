using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireball : MonoBehaviour
{
    AnElement this_element;
    // Start is called before the first frame update
    void Start()
    {
        this_element = gameObject.GetComponent<AnElement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            // this_element.element_if_active = !this_element.element_if_active;
            this_element.ElementIfActive = true;
        }
    }
}
