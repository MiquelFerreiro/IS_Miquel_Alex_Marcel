using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveBhvr : MonoBehaviour
{


    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.rigidbody.CompareTag("Enemy"))
        {
            EnemySpawner.remove_live(); 
            GameObject enemy = collision.gameObject;
            EnemySpawner.remove_enemy(enemy);
            Destroy(enemy);

        }


    }

}
