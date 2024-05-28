using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipAnimation : MonoBehaviour
{


    public float rotationSpeed = 720f; // 720 degrees per second for a full 360-degree rotation in 0.5 seconds

    // Timer to track when to rotate
    private float rotationTimer = 0f;

    private bool isRotating = false;

    public GameObject visual;

    public Material damage_material;

    private Renderer renderer;

    private Material original_material;
    
    public float yRotation = 60f;

    private float animation_progress = 0f;
    private const float ANIMATION_TIME = 3f; 
    private Quaternion start_rotation = Quaternion.Euler(45, 47, 45);
    private Quaternion anim_rotation = Quaternion.identity;

    void Start()
    {
        renderer = visual.GetComponent<Renderer>();

        original_material = renderer.material;
        start_rotation = transform.rotation;

    }

    void Update()
    {

        /*
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y + yRotation,
            transform.eulerAngles.z
        );*/

        //start_rotation = start_rotation * Quaternion.Euler(0, yRotation, 0);
        /*Vector3 rotation_temp = start_rotation.eulerAngles; 
        rotation_temp.y = + yRotation;
        transform.rotation = Quaternion.Euler(rotation_temp);
        */

        //start_rotation = start_rotation * Quaternion.Euler(0, yRotation * Time.deltaTime, 0);
        start_rotation = Quaternion.Euler(0, yRotation * Time.realtimeSinceStartup, 0) * Quaternion.Euler(45, 47, 45);
        transform.rotation = start_rotation * anim_rotation; 



        // Update the rotation timer if rotation is not in progress
        if (!isRotating)
        {
            rotationTimer += Time.deltaTime;

            // If 5 seconds have passed, start the rotation
            if (rotationTimer >= 5f)
            {
                isRotating = true;
                rotationTimer = 0f;
            }

        }
        else
        {

            animation_progress += Time.deltaTime;
            float x = Mathf.SmoothStep(0, 1, animation_progress / ANIMATION_TIME);
            //Debug.Log(animation_progress / ANIMATION_TIME); 
            //Debug.Log(x);
            anim_rotation = Quaternion.Euler(0, x * 360f, (1f - x) * 360f); 
            if (ANIMATION_TIME <= animation_progress)
            {
                isRotating = false;
                animation_progress = 0f;
            }
            
        }
    }
    public void ChangeMaterial()
    {
        // Change to the damage material
        renderer.material = damage_material;

        // Wait for 5 seconds
        StartCoroutine(WaitAndRevertMaterial());
    }

    IEnumerator WaitAndRevertMaterial()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(0.5f);

        // Revert back to the original material
        renderer.material = original_material;
    }


}
