using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Digit : MonoBehaviour
{
    public enum Operator
    {
        add,
        subtract,
        multiply,
        power,
        invalid // Can be used to check number of valid operations when casted to int
    };

    [SerializeField] private Renderer ref_renderer = null;
    [SerializeField] private TextMesh[] ref_text_digits = null;
    [SerializeField] private GameObject[] ref_underscores = null; // For 6 and 9 differentiation

    [SerializeField] private Color color_valid = Color.black;
    [SerializeField] private Color color_invalid = Color.black;
    [SerializeField] private Operator op = Operator.invalid;
    [SerializeField] private int value = 0;
    [SerializeField] private bool is_value = true;
    [SerializeField] private bool update = false;
    [SerializeField] private int total; // Don't change in inspector, for debugging

    private Behavior_Digit script_digit_upper = null;
    private Behavior_Digit script_digit_lower = null;
    private Color color_original;
    private bool valid_op = false; // Operator is only valid when there is an operand above it
    private bool valid_state = true; // Invalid state when op-op or operand-operand structure formed

    public bool GetIsValue() { return is_value; }
    public int GetTotal() { return total; }
    public Operator GetOperator() { return valid_op ? op : Operator.invalid; }

    public void SetValidState(bool vs) { valid_state = vs; }

    public void Initialize(Operator o)
    {
        op = o;
        is_value = false;
        update = true;
    }

    public void Initialize(int v)
    {
        value = v;
        if (v == 6 || v == 9)
        {
            foreach (GameObject go in ref_underscores)
            {
                go.SetActive(true);
            }
        }

        is_value = true;
        update = true;
    }

    public void SetColor(Color c)
    {
        ref_renderer.material.color = c;
    }

    public void UpdateColor()
    {
        if (script_digit_upper != null || script_digit_lower != null)
        {
            ref_renderer.material.color = valid_state ? color_valid : color_invalid;
        }
        else
        {
            ref_renderer.material.color = color_original;
        }
    }

    public void UpdateTotal()
    {
        int starting_total = total;

        // Defaults
        total = is_value ? value : 0;

        if (script_digit_upper != null)
        {
            Operator upper_op = script_digit_upper.GetOperator();
            if (script_digit_upper.GetIsValue() && !is_value)
            {
                // Above block is value and current block is operator, transfer total
                valid_op = true;
                valid_state = true;
                total = script_digit_upper.GetTotal();
            }
            else if (!script_digit_upper.GetIsValue() && is_value && upper_op != Operator.invalid)
            {
                // Above block is operator and current block is value, perform operation and calculate new total
                valid_state = true;
                total = ApplyOperation(upper_op, script_digit_upper.GetTotal(), value);
            }
            else // Invalid placement
            {
                valid_state = false;
                script_digit_upper.SetValidState(false);
                script_digit_upper.UpdateColor();
                UpdateColor();
                return;
            }
        }

        // Update lower block's total if exists
        if (script_digit_lower != null && starting_total != total)
        {
            script_digit_lower.UpdateTotal();
        }

        UpdateColor();
    }

    public static int ApplyOperation(Operator op, int x, int y)
    {
        switch (op)
        {
            case Operator.add:
                return x + y;
            case Operator.subtract:
                return x - y;
            case Operator.multiply:
                return x * y;
            case Operator.power:
                return x ^ y;
        }

        return 0;
    }

    private void Awake()
    {
        color_original = ref_renderer.material.color;
    }

    private void Start()
    {
        total = is_value ? value : 0;
    }

    private void Update()
    {
        if (update)
        {
            SetVisuals();
            UpdateTotal();
            update = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Digit")
        {
            Transform t = collision.transform;
            if (t.position.y > transform.position.y && script_digit_upper == null)
            {
                script_digit_upper = collision.gameObject.GetComponent<Behavior_Digit>();
                UpdateTotal();
            }
            else if (t.position.y < transform.position.y && script_digit_lower == null)
            {
                script_digit_lower = collision.gameObject.GetComponent<Behavior_Digit>();
                UpdateTotal();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Digit")
        {
            if (script_digit_upper != null && collision.gameObject.GetInstanceID() == script_digit_upper.gameObject.GetInstanceID())
            {
                script_digit_upper = null;
                valid_op = false;
                valid_state = true;
                UpdateTotal();
            }
            else if (script_digit_lower != null && collision.gameObject.GetInstanceID() == script_digit_lower.gameObject.GetInstanceID())
            {
                script_digit_lower = null;
                valid_state = true;
                UpdateTotal();
            }
        }
    }

    private void SetVisuals()
    {
        foreach (TextMesh tm in ref_text_digits)
        {
            if (is_value)
            {
                tm.text = value.ToString();
            }
            else
            {
                switch (op)
                {
                    case Operator.add:
                        tm.text = "+";
                        break;
                    case Operator.subtract:
                        tm.text = "-";
                        break;
                    case Operator.multiply:
                        tm.text = "x";
                        break;
                    case Operator.power:
                        tm.text = "^";
                        break;
                    default:
                        tm.text = "ERROR";
                        break;
                }                
            }
        }
    }
}
