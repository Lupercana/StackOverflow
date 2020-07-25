using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Behavior_Lose_Screen : MonoBehaviour
{
    [SerializeField] private Text ref_text_level = null;
    [SerializeField] private Object scene_main = null;

    private void Start()
    {
        ref_text_level.text = "ON LEVEL " + Behavior_State.Instance.state_level.ToString();
    }

    public void OnRetry()
    {
        SceneManager.LoadScene(scene_main.name);
    }
}
