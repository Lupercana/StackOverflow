using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Behavior_Manager : MonoBehaviour
{
    [SerializeField] private Text ref_text_level = null;
    [SerializeField] private Slider ref_slider_timer = null;
    [SerializeField] private Behavior_Spawner script_spawner = null;
    [SerializeField] private Behavior_Platform[] script_platforms = null;
    [SerializeField] private Object scene_lose = null;

    [SerializeField] private float level_time_seconds = 0f;
    [SerializeField] private int level = 0;
    [SerializeField] private int random_val_min = 0;
    [SerializeField] private int random_val_max = 1;
    [SerializeField] private int difficulty_raise_max = 1;
    [SerializeField] private int difficulty_raise_operations = 1;
    [SerializeField] private int difficulty_raise_platforms = 1;
    [SerializeField] private int difficulty_level_thresh_mult_op = 1;
    [SerializeField] private int difficulty_level_thresh_power_op = 1;

    private float start_time = 0f;
    private int uncompleted_platforms = 0;

    public int GetLevel() { return level; }

    public void PlatformComplete()
    {
        --uncompleted_platforms;

        if (uncompleted_platforms <= 0)
        {
            NextLevel();
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        NextLevel();
    }

    private void FixedUpdate()
    {
        // Timer updates
        float elasped_time = Time.time - start_time;
        if (elasped_time > level_time_seconds)
        {
            // Proceed to lose screen
            Behavior_State.Instance.state_level = level;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(scene_lose.name);
        }
        ref_slider_timer.value = 1f - (elasped_time / level_time_seconds);
    }

    private void NextLevel()
    {
        // Update level
        ++level;
        uncompleted_platforms = 0;
        ref_text_level.text = "Level " + level.ToString();

        // Destroy all cubes
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Digit"))
        {
            Destroy(go.gameObject);
        }

        // Difficulty adjustment
        int operations_used_max = ((level - 1) / difficulty_raise_operations) + 1;
        int operations_used = Random.Range(1, operations_used_max);

        int platforms_used = ((level - 1) / difficulty_raise_platforms) + 1;
        platforms_used = (platforms_used <= script_platforms.Length) ? platforms_used : script_platforms.Length;

        int max_inc = (level - 1) / difficulty_raise_max;

        // Update platforms and calculate cubes
        List<Behavior_Digit.Operator> digit_ops = new List<Behavior_Digit.Operator>();
        List<int> digit_values = new List<int>();
        for (int i = 0; i < script_platforms.Length; ++i)
        {
            if (i < platforms_used) // Platform in use
            {
                // Generate valid equation
                int total = Random.Range(random_val_min, random_val_max + max_inc);
                digit_values.Add(total);
                for (int j = 0; j < operations_used; ++j)
                {
                    int value = Random.Range(random_val_min, random_val_max + max_inc);
                    digit_values.Add(value);
                    Behavior_Digit.Operator op = RandomOperator();
                    digit_ops.Add(op);

                    total = Behavior_Digit.ApplyOperation(op, total, value);
                }

                script_platforms[i].Reset(total, true);
                ++uncompleted_platforms;
            }
            else // Platform not in use
            {
                script_platforms[i].Reset(0, false);
            }
        }

        // Spawn cubes, so they spawn all at once
        foreach (Behavior_Digit.Operator op in digit_ops)
        {
            script_spawner.SpawnOp(op);
        }
        foreach(int value in digit_values)
        {
            script_spawner.SpawnValue(value);
        }

        // Update variables
        start_time = Time.time;
    }

    private Behavior_Digit.Operator RandomOperator()
    {
        int total_ops = (int)Behavior_Digit.Operator.invalid;
        if (level < difficulty_level_thresh_power_op)
        {
            --total_ops; // Assume power op is last in enum
        }
        if (level < difficulty_level_thresh_mult_op)
        {
            --total_ops; // Assume mult op is second last in enum
        }

        return (Behavior_Digit.Operator) Random.Range(0, total_ops);
    }
}
