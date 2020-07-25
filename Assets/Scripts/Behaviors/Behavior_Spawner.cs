using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Spawner : MonoBehaviour
{
    
    [SerializeField] private Collider ref_spawner_box = null;
    [SerializeField] private GameObject ref_to_spawn = null;

    public void SpawnOp(Behavior_Digit.Operator o)
    {
        GameObject digit = Spawn();
        digit.GetComponent<Behavior_Digit>().Initialize(o);
    }

    public void SpawnValue(int v)
    {
        GameObject digit = Spawn();
        digit.GetComponent<Behavior_Digit>().Initialize(v);
    }

    private GameObject Spawn()
    {
        float spawn_x = Random.Range(ref_spawner_box.bounds.min.x, ref_spawner_box.bounds.max.x);
        float spawn_y = transform.position.y;
        float spawn_z = Random.Range(ref_spawner_box.bounds.min.z, ref_spawner_box.bounds.max.z);
        return Instantiate(ref_to_spawn, new Vector3(spawn_x, spawn_y, spawn_z), Quaternion.identity);
    }
}
