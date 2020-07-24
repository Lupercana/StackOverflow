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
        power
    };

    [SerializeField] private Renderer ref_renderer = null;

    [SerializeField] private Operator op = Operator.add;
    [SerializeField] private int value = 0;
    [SerializeField] private bool is_value = true;
    [SerializeField] private bool update = false;
    [SerializeField] private int total; // Don't change in inspector, for debugging

    public bool GetIsValue() { return is_value; }
    public int GetTotal() { return total; }
    public Operator GetOperator() { return op; }

    public void SetValue(Operator o)
    {
        op = o;
        is_value = false;
        SetVisuals();
    }

    public void SetValue(int v)
    {
        value = v;
        is_value = true;
        SetVisuals();
    }

    private void Start()
    {
        total = is_value ? value : 0;
    }

    private void Update()
    {
        if (update)
        {
            total = is_value ? value : 0;
            SetVisuals();
            update = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Digit")
        {
            ref_renderer.material.color = Color.white;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Digit")
        {
            ref_renderer.material.color = Color.yellow;
            Debug.Log("Set Yellow");
            if (collision.transform.position.y >= transform.position.y)
            {
                // Defaults
                total = is_value ? value : 0;

                Behavior_Digit script_other_digit = collision.gameObject.GetComponent<Behavior_Digit>();
                if (script_other_digit.GetIsValue() && !is_value)
                {
                    // Above block is value and current block is operator, transfer total
                    total = script_other_digit.GetTotal();
                }
                else if (!script_other_digit.GetIsValue() && is_value)
                {
                    // Above block is operator and current block is value, perform operation and calculate new total
                    total = ApplyOperation(script_other_digit.GetOperator(), script_other_digit.GetTotal(), value);
                }
                else // Invalid placement
                {
                    ref_renderer.material.color = Color.red;
                }
            }
        }
    }

    private int ApplyOperation(Operator op, int x, int y)
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

    private void SetVisuals()
    {
        foreach (TextMesh tm in GetComponentsInChildren<TextMesh>())
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
                }                
            }
        }
    }
}
