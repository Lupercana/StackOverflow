using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Player_Hands : MonoBehaviour
{
    [SerializeField] private Color color_select = Color.black;

    private GameObject ref_held = null;
    private GameObject ref_in_trigger = null;

    public void Grab()
    {
        if (ref_held == null) // Hands are empty, grab item if exists
        {
            if (ref_in_trigger != null) // Item exists
            {
                ref_held = ref_in_trigger;
            }
        }
        else // Hands are full, drop item
        {
            ref_held = null;
        }
    }

    private void Update()
    {
        if (ref_held != null)
        {
            ref_held.transform.position = transform.position;
            ref_held.GetComponent<Rigidbody>().velocity = GetComponentInParent<Rigidbody>().velocity;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Digit")
        {
            if (ref_in_trigger == null)
            {
                ref_in_trigger = other.gameObject;
            }
            if (other.gameObject.GetInstanceID() == ref_in_trigger.GetInstanceID())
            {
                other.gameObject.GetComponent<Behavior_Digit>().SetColor(color_select);
            }
        }
        else if (other.tag == "Cart")
        {
            if (ref_in_trigger == null)
            {
                ref_in_trigger = other.gameObject;
            }
            if (other.gameObject.GetInstanceID() == ref_in_trigger.GetInstanceID())
            {
                other.gameObject.GetComponent<Behavior_Cart>().SetColor(color_select);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Digit" || other.tag == "Cart") && ref_in_trigger != null && other.gameObject.GetInstanceID() == ref_in_trigger.gameObject.GetInstanceID())
        {
            ref_in_trigger = null;
            if (other.tag == "Digit")
            {
                other.gameObject.GetComponent<Behavior_Digit>().UpdateColor();
            }
            else if (other.tag == "Cart")
            {
                other.gameObject.GetComponent<Behavior_Cart>().ResetColor();
            }
        }
    }
}
