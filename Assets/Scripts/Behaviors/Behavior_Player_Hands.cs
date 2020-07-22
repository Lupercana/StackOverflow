using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Player_Hands : MonoBehaviour
{
    [SerializeField] private GameObject ref_held = null;

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
            ref_in_trigger = other.gameObject;

            if (ref_held == null)
            {
                other.GetComponent<Renderer>().material.color = Color.green;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Digit")
        {
            ref_in_trigger = null;
            other.GetComponent<Renderer>().material.color = Color.gray;
        }
    }
}
