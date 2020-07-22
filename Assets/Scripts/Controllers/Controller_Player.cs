using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Player : MonoBehaviour
{
    [SerializeField] private Behavior_Player_Hands script_hands = null;
    [SerializeField] private Rigidbody self_rbody = null;

    [SerializeField] private float look_sensitivity_x = 0f;
    [SerializeField] private float look_sensitivity_y = 0f;
    [SerializeField] private float walk_speed = 0f;
    [SerializeField] private float run_speed = 0f;
    [SerializeField] private float fly_speed = 0f;

    private float move_x = 0f;
    private float move_y = 0f;
    private float jump = 0f;
    private float move_speed = 0f;

    private void Start()
    {
        // Hide cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Movement Inputs
        move_x = Input.GetAxis("Vertical");
        move_y = Input.GetAxis("Horizontal");
        jump = Input.GetAxis("Jump");
        if (Input.GetButton("Run"))
        {
            move_speed = run_speed;
        }
        else
        {
            move_speed = walk_speed;
        }


        if (Input.GetButtonDown("Grab"))
        {
            script_hands.Grab();
        }

        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * look_sensitivity_x * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y") * look_sensitivity_y * Time.deltaTime, Space.Self);
    }

    private void FixedUpdate()
    {
        self_rbody.AddForce(transform.forward * move_x * move_speed * Time.fixedDeltaTime, ForceMode.Force);
        self_rbody.AddForce(transform.right * move_y * move_speed * Time.fixedDeltaTime, ForceMode.Force);
        self_rbody.AddForce(Vector3.up * jump * fly_speed * Time.fixedDeltaTime, ForceMode.Force);
    }
}
