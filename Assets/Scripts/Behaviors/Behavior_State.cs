using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_State : MonoBehaviour
{
    public static Behavior_State Instance = null;

    public int state_level { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
