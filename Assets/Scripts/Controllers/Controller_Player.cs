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

    private float move_forward = 0f;
    private float move_lateral = 0f;
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
        move_forward = Input.GetAxis("Vertical");
        move_lateral = Input.GetAxis("Horizontal");
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
        Vector3 movement_forward = transform.forward;
        Vector3 movement_lateral = transform.right;
        movement_forward.y = 0;
        movement_lateral.y = 0;

        self_rbody.AddForce((movement_forward.normalized * move_forward + movement_lateral.normalized * move_lateral).normalized * move_speed * Time.fixedDeltaTime, ForceMode.Force);
        self_rbody.AddForce(Vector3.up * jump * fly_speed * Time.fixedDeltaTime, ForceMode.Force);
    }
}
