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
    public const float MAX_SPEED = 10f;
    public const float SQUARED_MAX_SPEED = MAX_SPEED * MAX_SPEED;
    public const float DRAG_FORCE_MULTIPLIER = 5f; 

    /// <summary>
    /// ////////////////////  SPECIAL BALOON STUFF
    /// </summary>
    //NOTE: the prefix "SP_<type>_" means that this is a variable used only when the ballon is special and of type <type>

    //special variable that can serve multiple pruposes depending the special type of the baloon
    private float special_auxiliar = 0f; 
    // An enemy almost died recovers in 1/DEATH_TIMER_REPLENISMENT_RATE seconds
    //private const float SP_DOUBLE_DEATH_TIMER_REPLENISMENT_RATE = 1.1f; //greater than 1

    private float startTime;

    public GameObject Number2;

    private GameObject Player1;

    private GameObject Player2;

    private bool touchingP1;

    private bool touchingP2;

    /// <summary>
    /// ////////////////////  SPECIAL BALOON STUFF
    /// </summary>

    void Start()
    {
       
        rigid_body = GetComponent<Rigidbody>();

        OBJECTIVE = GameObject.Find("Objective");

        rigid_body.mass = Mathf.Clamp(Utils.GetNumberNormal() * 0.5f + 1, 0.01f, 100f);

        //rigid_body.velocity = new Vector3(-transform.position.z + 50, 0, transform.position.x - 50) * 1 / 10;
        //da errores... lo cambio por AddForce en Update

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
                //visual feedback Double
                Number2 = Instantiate(help.Number2);

                Number2.transform.parent = gameObject.transform;
                Number2.transform.localPosition = new Vector3(0, 1.5f, 0);
                Number2.transform.eulerAngles = new Vector3(90, 0, 0);


            } else
            {
                type = Type.Leaking;
                special_auxiliar = (1 - Random.value * Random.value) * 2f + 1.5f; 

            }//add more types here

        }

        startTime = Time.time;

        touchingP1 = false;
        touchingP2 = false;

        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");
    }

    void Update()
    {
        if(death_timer <= 0f) {
            EnemySpawner.remove_enemy(gameObject);
            Destroy(gameObject);

        }

        death_timer = Mathf.Clamp(death_timer + Time.deltaTime * DEATH_TIMER_REPLENISMENT_RATE, 0f, DEATH_TIMER_MAX);

        if (warning_timer <= 0f) {
            Debug.Log("NOT THE RIGHT HEIGHT!! ");
            warning_timer = WARNING_TIMER_MAX;

            EnemySpawner.SOUND_CONTROLLER.PlayWrongHeight();
        }


        Vector3 force_dir = OBJECTIVE.transform.position - transform.position;
        force_dir.y = 0;
        force_dir = force_dir.normalized;

        rigid_body.AddForce(1f * force_dir);

        // Fuerza paralela
        if (Time.time - startTime < 6f)
        {
            rigid_body.AddForce(0.5f * new Vector3(-force_dir.z, 0, force_dir.x));
        }
        
        if (type != Type.Simple)
        {
            handle_baloon_behaviour(); 
        }

        if(SQUARED_MAX_SPEED <= rigid_body.velocity.sqrMagnitude)
        {

            float signx = Mathf.Sign(rigid_body.velocity.x); 
            float signz = Mathf.Sign(rigid_body.velocity.z); 
            Vector3 speed_limiting_force = -rigid_body.velocity; 

            //TODO: Add limiting force

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
                    float impulse_strength = Utils.GetNumberNormal() * 0.75f + 2.5f;

                    rigid_body.AddForce(impulse_strength * dir);
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



        if (attack_height_enum == height)
        {

            if (type == Type.Double)
            {
                if (touchingP1 && touchingP2)
                {
                    death_timer += -Time.deltaTime;
                    visual.transform.localScale += Vector3.one * Time.deltaTime;
                }
                else
                {
                    warning_timer += -Time.deltaTime;
                }
                return;
            } 
            else 
            { 
            death_timer += -Time.deltaTime;
            visual.transform.localScale += Vector3.one * Time.deltaTime; 
            }

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

        if (other.gameObject == Player1)
        {
            touchingP1 = true;
        }
        else if (other.gameObject == Player2)
        {
            touchingP2 = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player1)
        {
            touchingP1 = false;
        }
        else if (other.gameObject == Player2)
        {
            touchingP2 = false;
        }
    }

}
