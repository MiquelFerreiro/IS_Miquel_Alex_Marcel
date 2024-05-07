using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AuxShowHeight : MonoBehaviour
{

    public const float IRL2UNITY = 100f / 6f; //conversion factor for distances between unity and irl. 
    public const float UNITY2IRL = 6f / 100f; //conversion factor for distances between unity and irl. 

    public GameObject p1; 
    public GameObject p2;
    public TextMeshProUGUI tmp; 

    void Start()
    {
        
    }

    void Update()
    {

        tmp.text = string.Format("{0}\n{1}", p1.transform.position.y * UNITY2IRL, p2.transform.position.y * UNITY2IRL); 

    }
}
