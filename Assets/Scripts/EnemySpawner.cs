using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemy_prefab;
    [SerializeField] private float min_spawn_radius = 0f; 
    [SerializeField] private float max_spawn_radius = 10f; 
    [SerializeField] private Vector3 spawn_center = Vector3.zero;
    public static List<GameObject> enemy_list = new List<GameObject>();
    [SerializeField] private uint max_enemy_count = 10;
    [SerializeField] private uint min_enemy_count = 0;
    [SerializeField] private float inter_enemy_spawn_dist = 5f; //distance to allow spawn between enemies
    [SerializeField] private float player_to_enemy_spawn_dist = 10f; //distance to allow spawn between enemies
    
    
    private const float fast_spawn_multiplier = 4f;
    private float time_for_enemy_spawn = 0f;
    private Transform player_1; 
    private Transform player_2;


    void Start()
    {
        if(enemy_prefab == null)
        {
            Debug.Log("Enemy prefab not setted. "); 
        }

        //get reference of player 1 and 2
        foreach(GameObject possible_player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(possible_player.name == "Player1")
            {
                player_1 = possible_player.transform;
                if (player_2 != null) break; 
            }

            if (possible_player.name == "Player2")
            {
                player_2 = possible_player.transform;
                if (player_1 != null) break;
            }
        }

    }

    void Update()
    {
        time_for_enemy_spawn += -Time.deltaTime; 
        //TODO: remove deleted elements
        int enemy_count = enemy_list.Count; 

        if(max_enemy_count <= enemy_count) return;

        if(enemy_count < min_enemy_count )
        {
            time_for_enemy_spawn = -Time.deltaTime * (fast_spawn_multiplier - 1); 
            //object spawns 4 times as fast

        } else if(enemy_count < max_enemy_count)
        {
            //dont do anything.
            //Let the compiler optimize this
        }

        if (time_for_enemy_spawn <= 0)
        {
            spawn_enemy();
            time_for_enemy_spawn = time_for_next_spawn();
        }

    }

    void spawn_enemy()
    {
        float rato_radius = min_spawn_radius / max_spawn_radius;
        float x;
        float y;
        do
        {
            /*
             *  Compute a random point in [-1, 1] x [-1, 1] and 
             *  use rejection method to select appropiate points 
             *  inside the donut. 
            */

            x = Random.value * 2 - 1; 
            y = Random.value * 2 - 1;
            float magnitude = x * x + y * y; 

            if (1 < magnitude)
            {
                //outside the outer circle, reject. 
                continue; 
            }

            if(magnitude < rato_radius)
            {
                //inside the inner circle, reject
                continue; 
            }

            Vector3 coordinate = new Vector3(x * max_spawn_radius, 0, y * max_spawn_radius); // in world space
            Vector3 displacement; //auxiliar variable
            int enemy_count = enemy_list.Count;
            bool reject = false; 
            for (int i = 0; i < enemy_count; i++)
            {

                displacement = enemy_list[i].transform.position - coordinate; 
                if(displacement.sqrMagnitude <= inter_enemy_spawn_dist * inter_enemy_spawn_dist)
                {
                    reject = true; 
                    break;
                }

            }

            if (reject) continue;

            displacement = player_1.position - coordinate; 
            if(displacement.sqrMagnitude < player_to_enemy_spawn_dist * player_to_enemy_spawn_dist)
            {
                // too near to player 1, reject
                continue; 
            }

            displacement = player_2.position - coordinate;
            if (displacement.sqrMagnitude < player_to_enemy_spawn_dist * player_to_enemy_spawn_dist)
            {
                // too near to player 2, reject
                continue;
            }

            //points are valid, accept
            break; 


        } while (true);

        //remap position from [-1, 1] x [-1, 1] to [-outer_radius, outer_radius] x [-outer_radius, outer_radius]
        x = x * max_spawn_radius; 
        y = y * max_spawn_radius; 

        Vector3 pos = spawn_center + new Vector3(x, 0, y); //I lied. y was z all the time. 
        GameObject new_enemy = Instantiate(enemy_prefab, pos, Quaternion.identity); 

        new_enemy.transform.localScale = Vector3.one * ((Random.value * Random.value - 0.5f) * 0.3f + 1f); 
        // add tiny change in scale

        enemy_list.Add(new_enemy); 

    }

    float time_for_next_spawn()
    {
        //TODO: use cool random distribution
        return Random.value * Random.value * 6f + 2f; 
    }
}
