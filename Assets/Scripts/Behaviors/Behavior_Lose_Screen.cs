using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Behavior_Lose_Screen : MonoBehaviour
{
    [SerializeField] private Object scene_main = null;

    public void OnRetry()
    {
        SceneManager.LoadScene(scene_main.name);
    }
}
