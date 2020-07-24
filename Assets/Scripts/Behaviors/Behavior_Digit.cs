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

    [SerializeField] private Operator op = Operator.add;
    [SerializeField] private int value = 0;
    [SerializeField] private bool is_value = true;
    [SerializeField] private bool update = false;

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

    private void Update()
    {
        if (update)
        {
            SetVisuals();
            update = false;
        }
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
