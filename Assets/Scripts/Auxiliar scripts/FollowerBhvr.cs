using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerBhvr : MonoBehaviour
{
    [SerializeField] private GameObject asset;

    private void Start()
    {
        Instantiate(asset);
    }
    void Update()
    {
        asset.transform.position = transform.position;
    }
}
