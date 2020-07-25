using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Platform : MonoBehaviour
{
    [SerializeField] private Renderer ref_renderer = null;
    [SerializeField] private TextMesh[] ref_text_actual = null;
    [SerializeField] private TextMesh[] ref_text_desired = null;
    [SerializeField] private Behavior_Manager script_manager = null;

    [SerializeField] private Color color_completed = Color.black;

    [SerializeField] private Behavior_Digit script_digit_connected = null;

    private Color color_original;
    private int value_actual = 0;
    private int value_desired = 0;
    private int value_displayed = 0;
    private bool completed = true;

    public void Reset(int new_desired, bool used)
    {
        value_actual = 0;
        completed = !used;
        ref_renderer.material.color = used ? color_original : color_completed;
        if (used)
        {
            value_desired = new_desired;
            UpdateText(ref_text_actual, "");
            UpdateText(ref_text_desired, value_desired.ToString());
        }
        else
        {
            UpdateText(ref_text_actual, "");
            UpdateText(ref_text_desired, "");
        }
    }

    private void Awake()
    {
        color_original = ref_renderer.material.color;
    }

    private void Update()
    {
        if (!completed && script_digit_connected != null)
        {
            value_actual = script_digit_connected.GetTotal();

            if (value_actual != value_displayed) // Save CPU
            {
                CheckTotal();
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Digit" && script_digit_connected == null)
        {
            script_digit_connected = collision.gameObject.GetComponent<Behavior_Digit>();
            if (!script_digit_connected.GetIsValue()) // Don't bind to operators
            {
                script_digit_connected = null;
            }
            else
            {
                value_actual = script_digit_connected.GetTotal();
                CheckTotal();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Digit" && script_digit_connected != null &&
            collision.gameObject.GetInstanceID() == script_digit_connected.gameObject.GetInstanceID())
        {
            script_digit_connected = null;
            UpdateText(ref_text_actual, "");
        }
    }

    private void CheckTotal()
    {
        UpdateText(ref_text_actual, value_actual.ToString());
        value_displayed = value_actual;

        if (value_actual == value_desired)
        {
            // Completed
            completed = true;
            ref_renderer.material.color = color_completed;
            script_manager.PlatformComplete();
        }
    }

    private void UpdateText(TextMesh[] texts, string new_value)
    {
        foreach (TextMesh tm in texts)
        {
            tm.text = new_value;
        }
    }
}
