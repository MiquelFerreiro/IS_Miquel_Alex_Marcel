using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static float IRL2UNITY = 100f / 6f; //conversion factor for distances between unity and irl. 
    public static float UNITY2IRL = 6f / 100f; //conversion factor for distances between unity and irl. 
    
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

    public enum Type
    {
        Simple, //nada especial
        Double, //tarda el doble en matarse
        Leaking, //recive impulsos aleatorios
    }
    public static GameObject OBJECTIVE; 

    public const float LOW_MEDIUM_BOUNDARY = 0.85f; //Boundary between low and medium height
    public const float MEDIUM_HIGH_BOUNDARY = 1.4875f; //Boundary between medium and high height
    public const float SPECIAL_BALOON_CHANCE = 0.2f; //Chance to get a special baloon instead of a regular one


    public Height height; 
    public Type type;
    //private float base_size;
    public GameObject visual;

    private const float DEATH_TIMER_MAX = 0.6f; //The time it takes for an enemy to die. In seconds, oviously
    private const float DEATH_TIMER_REPLENISMENT_RATE = 0.3f; // An enemy almost died recovers in 1/DEATH_TIMER_REPLENISMENT_RATE seconds
    private float death_timer = DEATH_TIMER_MAX; //time in seconds

    private const float WARNING_TIMER_MAX = 1.2f;
    private float warning_timer = WARNING_TIMER_MAX;

    private Rigidbody rigid_body;

    /// <summary>
    /// ////////////////////  SPECIAL BALOON STUFF
    /// </summary>
    //NOTE: the prefix "SP_<type>_" means that this is a variable used only when the ballon is special and of type <type>

    //special variable that can serve multiple pruposes depending the special type of the baloon
    private float special_auxiliar = 0f; 
    // An enemy almost died recovers in 1/DEATH_TIMER_REPLENISMENT_RATE seconds
    private const float SP_DOUBLE_DEATH_TIMER_REPLENISMENT_RATE = 1.1f; //greater than 1

    /// <summary>
    /// ////////////////////  SPECIAL BALOON STUFF
    /// </summary>

    void Start()
    {
       
        rigid_body = GetComponent<Rigidbody>();

        OBJECTIVE = GameObject.Find("Objective");

        rigid_body.mass = Mathf.Clamp(Utils.GetNumberNormal() * 0.5f + 1, 0.01f, 100f);
        rigid_body.velocity = new Vector3(-transform.position.z + 50, transform.position.y, transform.position.x - 50) * 1 / 10;

        float p = Random.value;

        if(p < 0.333333f) {
            height = Height.Low;
            gameObject.transform.position += new Vector3(0, 0.3f, 0); // * IRL2UNITY; //added height for player visual feedback
        } else if(p < 0.666666f) { 
            height = Height.Medium;
            gameObject.transform.position += new Vector3(0, 0.8f, 0); // * IRL2UNITY;
        } else {
            height = Height.High;
            gameObject.transform.position += new Vector3(0, 2f, 0); // * IRL2UNITY;
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

        //if (false) // testing
        if (SPECIAL_BALOON_CHANCE < Random.value)
        {
            type = Type.Simple;
        } else
        {
            p = Random.value;
            if(p <= 0.5f)
            {
                type = Type.Double; 
            } else
            {
                type = Type.Leaking;
                special_auxiliar = (1 - Random.value * Random.value) * 2f + 1.5f; 

            }//add more types here

        }
        /*
        if (!GameObject.Find("PluginController").GetComponent<PluginConnector>().get_is_tracking_enabled())
        {
            Poner factores de conversión si se controla an local
            EnemyController.IRL2UNITY = 100f / 6f; 
            
            EnemyController.UNITY2IRL = 6f / 100f;
        }
        */

    }

    void Update()
    {
        if(death_timer <= 0f) {
            EnemySpawner.remove_enemy(gameObject);
            Destroy(gameObject);

        }
        if (type == Type.Double)
        {
            death_timer = Mathf.Clamp(death_timer + Time.deltaTime * SP_DOUBLE_DEATH_TIMER_REPLENISMENT_RATE, 0f, DEATH_TIMER_MAX);
        } else
        {
            death_timer = Mathf.Clamp(death_timer + Time.deltaTime * DEATH_TIMER_REPLENISMENT_RATE, 0f, DEATH_TIMER_MAX); 
        }
    
        if(warning_timer <= 0f) {
            Debug.Log("NOT THE RIGHT HEIGHT!! ");
            warning_timer = WARNING_TIMER_MAX;

            EnemySpawner.SOUND_CONTROLLER.PlayWrongHeight();
        }

        if(true) //this if will get optimized out
        {

            Vector3 force_dir = OBJECTIVE.transform.position - transform.position;
            force_dir.y = 0;
            force_dir = force_dir.normalized;

            rigid_body.AddForce(0.5f * force_dir);
            //rigid_body.velocity = force_dir*5;
        } else
        {

            //Lines limits considered: 
            //(1): x = 0
            //(2): y = 0
            //(3): x - 100 = 0
            //(4): y - 100 = 0
            //https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line

            //float num = transform.position; 
            //float den = 1; //denominator is always 1 i npur case
            float distance_to_limit = transform.position.x; //(1)

            distance_to_limit = transform.position.y; //(2)
 

            //TODO: aply force from exterior

        }

        if(type != Type.Simple)
        {
            handle_baloon_behaviour(); 
        }

    }


    void handle_baloon_behaviour()
    {

        transform.localScale = Vector3.one * (1f + 0.2f * Mathf.Sin(Time.realtimeSinceStartup * 4f));
        //TODO: ^remove this. Testing only, to easly detect special baloons

        switch (type)
        {
            case Type.Simple: 
                //UNREACHABLE
                break;
            case Type.Double:
                break;
            case Type.Leaking:
                special_auxiliar += -Time.deltaTime;
                if (special_auxiliar <= 0f)
                {
                    special_auxiliar = (1 - Random.value * Random.value) * 2f + 1.5f;
                    float theta = Random.value * 2 * Mathf.PI;
                    Vector3 dir = new Vector3(Mathf.Cos(theta), 0f, Mathf.Sin(theta));
                    float impulse_strength = Utils.GetNumberNormal() * 2f + 5f;

                    rigid_body.AddForce(impulse_strength * dir, ForceMode.Impulse);
                }
                break;
            default:
                Debug.LogError("Switch statement is not exhaustive. "); //no quitar
                break;

        }

    }

    private void OnTriggerStay(Collider other)
    {
        //https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerStay.html
        //OnTriggerStay is called once per physics update for every Collider other that is touching the trigger.

        if (!other.CompareTag("Player")) return;

        if(type != Type.Simple)
        {
            death_timer += -Time.deltaTime;
            return; 
        }

        float attack_height = other.transform.position.y; // * UNITY2IRL;
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
            death_timer += -Time.deltaTime; 

        } else {
            //the user is atacking at the wrong spot
            warning_timer += -Time.deltaTime; 
        }


        //Note: Happy secundary effect: if more than 1 player is touching the enemy, it dies way faster. 

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            EnemySpawner.remove_live();
            EnemySpawner.remove_enemy(gameObject);
            Destroy(gameObject);

        }

    }



}
