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

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        //Debug.Log("OnTriggerEnter"); 
        if (!other.CompareTag("Player")) return;


        EnemySpawner.remove_enemy(gameObject); 

        Destroy(gameObject);



    }
}
