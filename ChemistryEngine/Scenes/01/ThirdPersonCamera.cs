using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    public Transform lookAt;
    public Transform camTransform;
    public float distance = 3.0f;
    public float deltaX = 1.0f;
    public float deltaY= 1.0f;
    public float deltaZ = 1.0f;

    private float currentX = 0.0f;
    private float currentY = 45.0f;
    private float sensitivityX = 20.0f;
    private float sensitivityY = 20.0f;
    private Transform Player;
    public float speed = 1f;
    private void Start()
    {
        camTransform = transform;
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // private void Update()
    // {
    //     Vector3 targetPos = this.Player.transform.position + new Vector3(0, 2.42f, -2.42f);
    //     transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
    //     // 这两个位置不能反哦
    //     Quaternion targetRotation = Quaternion.LookRotation(Player.position - transform.position);
    //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    // }
    // private void Update()
    // {

    //     // currentX += Input.GetAxis("Mouse X")/2;
    //     // currentY += Input.GetAxis("Mouse Y")/2;

    //     // currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    // }

    private void LateUpdate()
    {
        Vector3 dir = lookAt.forward * distance + new Vector3(deltaX, deltaY, deltaZ);
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        // Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        // camTransform.position = lookAt.position + rotation * dir;
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }
}
