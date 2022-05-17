using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    private Animator anim;
    private CharacterController controller;

    public float speed = 600.0f;
    public float turnSpeed = 400.0f;
    private Vector3 moveDirection = Vector3.zero;
    public float gravity = 20.0f;

    public GameObject[] SkillBallPrefabs = new GameObject[2];

    public Rigidbody[] SkillBalls = new Rigidbody[2];
    private Vector3[] _skillBallsPos = new Vector3[2];
    private Quaternion[] _skillBallsRot = new Quaternion[2];
    public bool[] IsFlying = new bool[2];
    public float Power = 1000.0f;
    private string[] _matNames = new string[] { "Fire", "Water" };

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < 2; i++)
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
        _skillBallsPos[i] = SkillBalls[i].gameObject.transform.position;
        _skillBallsRot[i] = SkillBalls[i].gameObject.transform.rotation;
        var newSkillBall = Instantiate(SkillBallPrefabs[i], _skillBallsPos[i], _skillBallsRot[i]);
        Debug.Log(newSkillBall);
        newSkillBall.layer = 0;

        Material sourceMat = Resources.Load<Material>("Materials/" + _matNames[i]);
        Debug.Log(sourceMat);
        Shader shader = sourceMat.shader;
        Material tmpmaterial = new Material(shader);
        tmpmaterial.CopyPropertiesFromMaterial(sourceMat);
        newSkillBall.GetComponent<MeshRenderer>().material = tmpmaterial;

        newSkillBall.GetComponent<Rigidbody>().isKinematic = false;
        newSkillBall.GetComponent<Rigidbody>().AddForce(transform.forward * Power, ForceMode.Acceleration);
        newSkillBall.GetComponent<Rigidbody>().useGravity = true;
    }

    void Start()
    {
        foreach (var skillBall in SkillBalls)
        {
            skillBall.isKinematic = true;
            skillBall.useGravity = false;
        }
        for (int i = 0; i < 2; i++)
        {
            IsFlying[i] = false;
        }

        controller = GetComponent<CharacterController>();
        anim = gameObject.GetComponentInChildren<Animator>();
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

        if (Input.GetKey("w"))
        {
            anim.SetInteger("AnimationPar", 1);
        }
        else if (Input.GetKey("s"))
        {
            anim.SetInteger("AnimationPar", -1);
        }
        else
        {
            anim.SetInteger("AnimationPar", 0);
        }

        if (controller.isGrounded)
        {
            moveDirection = transform.forward * Input.GetAxis("Vertical") * speed;
        }

        float turn = Input.GetAxis("Horizontal");
        transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
        controller.Move(moveDirection * Time.deltaTime);
        moveDirection.y -= gravity * Time.deltaTime;

        // float h = Input.GetAxis("Horizontal");
        // float v = Input.GetAxis("Vertical");

        // if(Mathf.Abs(h) >= 0.01 || Mathf.Abs(v) >= 0.01)
        // {
        //     Vector3 dir = new Vector3(h, 0, v);

        //     transform.LookAt(transform.position + dir);

        //     controller.SimpleMove(transform.forward * speed);

        // }
    }

    public void Fly()
    {
        anim.SetInteger("IsFlying", 1);
    }
    public void StopFly()
    {
        anim.SetInteger("IsFlying", 0);
    }
}
