using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Cart : MonoBehaviour
{
    [SerializeField] private Rigidbody ref_rbody = null;
    [SerializeField] private Renderer[] ref_renderers = null;

    private Color color_original;

    public void SetColor(Color c)
    {
        foreach (Renderer r in ref_renderers)
        {
            r.material.color = c;
        }
    }

    public void ResetColor()
    {
        foreach (Renderer r in ref_renderers)
        {
            r.material.color = color_original;
        }
    }

    private void Awake()
    {
        color_original = ref_renderers[0].material.color;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Digit")
        {
            other.GetComponent<Rigidbody>().velocity = ref_rbody.velocity;
        }
    }
}
