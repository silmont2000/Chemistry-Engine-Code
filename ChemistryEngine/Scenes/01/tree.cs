using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tree : RecieverBase
{
    float _per = 0.0f;
    bool _fireFlag = false;
    bool _waterFlag = false;

    ParticleSystem _particle = null;
    Color _finalBurnedColor = new Color(0.2735849f, 0.03200426f, 0.0592805f, 1);
    Color _finalWetColor = new Color(0.0242791f, 0.08686747f, 0.2075472f, 1);
    // Environment _env = new Environment();
    // ASurrounding this_surrounding;
    // Start is called before the first frame update
    void Start()
    {
        Bound();
        var test = gameObject.GetComponentsInChildren<ParticleSystem>();
        if (test.Length > 0)
        {
            _particle = test[0];
            var emission = _particle.emission;
            emission.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Color initial = new Color(0.8627f, .8627f, 0.7019608f, 1);
        if (_fireFlag)
        {
            var mats = gameObject.GetComponent<Renderer>().materials;
            foreach (var mat in mats)
            {
                // Debug.Log(mat.name);
                if (mat.name == "Tree1 (Instance)" || mat.name == "Tree2 (Instance)")
                {
                    mat.SetFloat("_DissolvePercentage", _per);
                }
                else
                {
                    Color tmp = mat.GetColor("_Color");
                    mat.SetColor("_Color", tmp * (1 - _per) + _finalBurnedColor * (_per));
                }

            }
            _per += 0.001f;
            if (_per >= 1.0)
            {
                _fireFlag = false;
                AnElement tmp;
                if ((tmp = gameObject.GetComponent<AnElement>()) != null)
                    tmp.ElementIfActive = false;
                if (_particle != null)
                {
                    StartCoroutine("StopPartical");
                }

            }
        }
        else if (_waterFlag)
        {
            var mats = gameObject.GetComponent<Renderer>().materials;
            foreach (var mat in mats)
            {
                if (mat.name == "Tree1 (Instance)" || mat.name == "Tree2 (Instance)")
                {
                    Color tmp = mat.GetColor("_Color");
                    mat.SetColor("_Color", tmp * (1 - _per) + _finalWetColor * (_per));
                    mat.SetFloat("_DissolvePercentage", _per);
                }
                else
                {
                    Color tmp = mat.GetColor("_Color");
                    mat.SetColor("_Color", tmp * (1 - _per) + _finalWetColor * (_per));
                }

            }
            _per += 0.0005f;
            if (_per >= 0.4)
            {
                _waterFlag = false;
                _per = 0.4f;
                // gameObject.GetComponent<AnElement>().ElementIfActive = false;
                if (_particle != null)
                {
                    StartCoroutine("StopPartical");
                }
            }

            // _finalWetColor
        }
    }


    IEnumerator StopPartical()
    {
        var m_MaxLifetime = _particle.main.startLifetime.constant;
        var emission = _particle.emission;
        emission.enabled = false;
        yield return new WaitForSeconds(m_MaxLifetime);
        Destroy(_particle);
    }
    public override void OnFire(AnElement e)
    {
        _fireFlag = true;
        if (_particle != null)
        {
            //     var main = _particle.main;
            //     main.loop = false;
            var emission = _particle.emission;
            emission.enabled = true;
            Material sourceMat = Resources.Load<Material>("Materials/OnFire");
            // _particle.renderer.material = sourceMat;
            _particle.gameObject.GetComponent<ParticleSystemRenderer>().material = sourceMat;
        }
    }

    public override void OnFireWater(AnElement e)
    {
        _fireFlag = false;
        _waterFlag = true;
        if (_particle != null)
        {
            _particle.gameObject.transform.position += new Vector3(0, 1, 0);
            var main = _particle.main;
            main.loop = false;
            Material sourceMat = Resources.Load<Material>("Materials/OnFireWater");
            _particle.gameObject.GetComponent<ParticleSystemRenderer>().material = sourceMat;
        }
    }

    // private void OnCollisionEnter(Collision other)
    // {
    // Debug.Log(gameObject.name);
    // }
}
