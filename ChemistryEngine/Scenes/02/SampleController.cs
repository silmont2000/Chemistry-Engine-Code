using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleController : MonoBehaviour
{
    public Rigidbody[] SkillBalls = new Rigidbody[3];
    public bool[] IsFlying = new bool[3];
    public float Power = 10.0f;
    private
    // Start is called before the first frame update
    void Start()
    {
        foreach (var skillBall in SkillBalls)
        {
            skillBall.isKinematic = true;
            skillBall.useGravity = false;
        }
        for (int i = 0; i < 3; i++)
        {
            IsFlying[i] = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < 3; i++)
        {
            if (IsFlying[i] == true)
            {
                // SkillBalls[i].AddForce
            }
            IsFlying[i] = false;
        }
    }
    void Fly(int i)
    {
        Debug.Log(i);
        SkillBalls[i].isKinematic = false;
        SkillBalls[i].AddForce(new Vector3(0, 0, 1) * Power, ForceMode.Acceleration);
        SkillBalls[i].useGravity = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Fly(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Fly(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Fly(2);
        }
    }
}
