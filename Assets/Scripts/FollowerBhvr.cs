using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerBhvr : MonoBehaviour
{
    [SerializeField] private GameObject idol;

    void Update()
    {
        transform.position = new Vector3(idol.transform.position.x, 0f, idol.transform.position.z);
    }
}
