using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float grow_speed = 0.1f;
    public float timeToLive = 6f;
    

    private float base_size;

    // Start is called before the first frame update
    void Start()
    {
        base_size = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x < 3 * base_size)
        {
            //transform.localScale *= 1 + grow_speed * Time.deltaTime;
            transform.localScale *= 1 + grow_speed * Time.time;
        }
        
    }
}
