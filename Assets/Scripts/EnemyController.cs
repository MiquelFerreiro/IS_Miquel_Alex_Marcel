using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //public const float IRL2UNITY = 100f / 6f; //conversion factor for distances between unity and irl. 
    //public const float UNITY2IRL = 6f / 100f; //conversion factor for distances between unity and irl. 
    public static float IRL2UNITY = 1f; //conversion factor for distances between unity and irl. 
    public static float UNITY2IRL = 1f; //conversion factor for distances between unity and irl. 
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

    public static GameObject OBJECTIVE; 

    public const float LOW_MEDIUM_BOUNDARY = 0.85f; //Boundary between low and medium height
    public const float MEDIUM_HIGH_BOUNDARY = 1.4875f; //Boundary between medium and high height


    public Height height; 
    private float base_size;
    public GameObject visual;

    private const float DEATH_TIMER_MAX = 0.6f; //The time it takes for an enemy to die. In seconds, oviously
    private const float DEATH_TIMER_REPLENISMENT_RATE = 0.5f; // An enemy almost died recovers in 1/DEATH_TIMER_REPLENISMENT_RATE seconds
    private float death_timer = DEATH_TIMER_MAX; //time in seconds

    private const float WARNING_TIMER_MAX = 2f;
    private float warning_timer = WARNING_TIMER_MAX;

    private Rigidbody rigid_body; 

    void Start()
    {

        rigid_body = GetComponent<Rigidbody>(); 

        float p = Random.value;

        if(p < 0.333333f) {
            height = Height.Low; 
        } else if(p < 0.666666f) { 
            height = Height.Medium; 
        } else {
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


        if(!GameObject.Find("PluginController").GetComponent<PluginConnector>().get_is_tracking_enabled())
        {
            //Poner factores de conversión si se controla an local
            EnemyController.IRL2UNITY = 100f / 6f; 
            EnemyController.UNITY2IRL = 6f / 100f;
        }

        rigid_body.velocity = new Vector3(-transform.position.z + 50, transform.position.y, transform.position.x - 50) * 1/10; 

    }

    void Update()
    {
        if(death_timer <= 0f) {
            EnemySpawner.remove_enemy(gameObject);
            Destroy(gameObject);

        }
        
        death_timer = Mathf.Clamp(death_timer + Time.deltaTime * DEATH_TIMER_REPLENISMENT_RATE, 0f, DEATH_TIMER_MAX); 
    
        if(warning_timer <= 0f) {
            Debug.Log("NOT THE RIGHT HEIGHT!! ");
            warning_timer = WARNING_TIMER_MAX;

            EnemySpawner.SOUND_CONTROLLER.PlayWrongHeight();
        }

        Vector3 force_dir = OBJECTIVE.transform.position - transform.position; 
        //force_dir = force_dir.normalized;

        rigid_body.AddForce(30 * force_dir.normalized / force_dir.magnitude);
        //rigid_body.velocity = force_dir*5;
    }

    private void OnTriggerStay(Collider other)
    {
        //https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerStay.html
        //OnTriggerStay is called once per physics update for every Collider other that is touching the trigger.

        if (!other.CompareTag("Player")) return;

        float attack_height = other.transform.position.y * UNITY2IRL;
        Height attack_height_enum; 

        if (attack_height <= LOW_MEDIUM_BOUNDARY) //Low
        {
            attack_height_enum = Height.Low; 
        }
        else if (attack_height <= MEDIUM_HIGH_BOUNDARY) //mid
        {
            attack_height_enum = Height.Medium;
        }
        else //high
        {
            attack_height_enum = Height.High;
        }



        if(attack_height_enum == height)
        {
            death_timer += -Time.deltaTime * (1 + DEATH_TIMER_REPLENISMENT_RATE); 

        } else {
            //the user is atacking at the wrong spot
            warning_timer += -Time.deltaTime; 
        }


        //Note: Happy secundary effect: if more than 1 player is touching the enemy, it dies way faster. 

    }




}
