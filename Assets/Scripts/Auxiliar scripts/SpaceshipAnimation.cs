using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipAnimation : MonoBehaviour
{

    public float yRotation = 1f;

    public float rotationSpeed = 720f; // 720 degrees per second for a full 360-degree rotation in 0.5 seconds

    // Timer to track when to rotate
    private float rotationTimer = 0f;

    private bool isRotating = false;

    private int num_rotations = 0;

    public GameObject visual;

    public Material damage_material;

    private Renderer renderer;

    private Material original_material;

    void Start()
    {
        renderer = visual.GetComponent<Renderer>();

        original_material = renderer.material;
    }

    void Update()
    {
    
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y + yRotation,
            transform.eulerAngles.z
        );
        

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
            // Rotate the object continuously
            transform.Rotate(Vector3.up, 10f);

            num_rotations++;

            // Check if the rotation is complete
            if (num_rotations == 36)
            {
                isRotating = false;
                num_rotations = 0;
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

    public void StartRotation()
    {
        isRotating = true;
        rotationTimer = 0f;
    }

    public void RotateObject()
    {
        transform.Rotate(Vector3.up, 10f);
        num_rotations++;

        if (num_rotations == 36) // Una rotación completa
        {
            FinishRotation();
        }
    }

    public void FinishRotation()
    {
        isRotating = false;
        num_rotations = 0;
    }


}
