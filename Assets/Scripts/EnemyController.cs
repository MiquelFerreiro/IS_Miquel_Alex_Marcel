using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Por abajo de cadera es abajo; por encima de hombros es arriba
    public enum Height { 
        Low, 
        /* red: infierno
         * Rango [0, 0.85] m
         * */
        Medium,
        /* green: tierra
         * Rango [0.85, 1.4875] m
         * */
        High
        /* blue: cielo
         * Rango [1.4875, 4] m 
         * */
    };

    public Height height; 
    private float base_size;
    public GameObject visual;

    private const float DEATH_TIMER_MAX = 1f; //The time it takes for an enemy to die
    private const float DEATH_TIMER_REPLENISMENT_RATE = 0.5f; // An enemy almost died recovers in 1/DEATH_TIMER_REPLENISMENT_RATE seconds
    private float death_timer = DEATH_TIMER_MAX; //time in seconds

    void Start()
    {
        float p = Random.value;

        if(p < 0.333333f)
        {
            height = Height.Low; 
        } else if(p < 0.666666f) { 
            height = Height.Medium; 
        } else
        {
            height = Height.High; 
        }

        SpawnerHelp help = GameObject.Find("GameController").GetComponent<SpawnerHelp>(); 
        switch ( height )
        {
            case Height.Low:

                visual = Instantiate( help.red_visual_enemy);
                break;
            case Height.Medium:

                visual = Instantiate(help.green_visual_enemy);
                break; 
            case Height.High:

                visual = Instantiate(help.blue_visual_enemy);
                break; 
        }
        visual.transform.parent = gameObject.transform;
        visual.transform.localPosition = Vector3.zero; 
        visual.transform.localRotation = Quaternion.identity;

    }

    void Update()
    {
        if(death_timer <= 0f) {
            EnemySpawner.remove_enemy(gameObject);
            Destroy(gameObject); 
        }
        
        death_timer = Mathf.Clamp(death_timer + Time.deltaTime * DEATH_TIMER_REPLENISMENT_RATE, 0f, DEATH_TIMER_MAX); 
    }

    private void OnTriggerStay(Collider other)
    {
        //https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerStay.html
        //OnTriggerStay is called once per physics update for every Collider other that is touching the trigger.

        if (!other.CompareTag("Player")) return;

        death_timer += -Time.deltaTime * (1 + DEATH_TIMER_REPLENISMENT_RATE); 

        //Note: Happy secundary effect: if more than 1 player is touching the enemy, it dies way faster. 

    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter"); 
        if (!other.CompareTag("Player")) return;

        EnemySpawner.remove_enemy(gameObject); 

        Destroy(gameObject);

    }*/


}
