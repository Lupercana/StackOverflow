using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Platform : MonoBehaviour
{
    [SerializeField] private TextMesh[] ref_text_actual = null;
    [SerializeField] private TextMesh[] ref_text_desired = null;

    private Behavior_Digit script_digit_connected = null;

    private int value_actual = 0;
    private int value_desired = 0;
    private int value_previous = 0;

    public void SetDesired(int new_value)
    {
        value_desired = new_value;
        UpdateText(ref_text_desired, value_desired);
    }

    private void Update()
    {
        if (script_digit_connected != null)
        {
            value_actual = script_digit_connected.GetTotal();
        }
        if (value_actual != value_previous)
        {
            UpdateText(ref_text_actual, value_actual);
        }
        value_previous = value_actual;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Digit" && script_digit_connected == null)
        {
            script_digit_connected = collision.gameObject.GetComponent<Behavior_Digit>();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Digit" && script_digit_connected != null &&
            collision.gameObject.GetInstanceID() == script_digit_connected.gameObject.GetInstanceID())
        {
            script_digit_connected = null;
            value_actual = 0;
        }
    }

    private void UpdateText(TextMesh[] texts, int new_value)
    {
        foreach (TextMesh tm in texts)
        {
            tm.text = new_value.ToString();
        }
    }
}
