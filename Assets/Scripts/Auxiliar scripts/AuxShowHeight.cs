using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AuxShowHeight : MonoBehaviour
{


    public GameObject p1; 
    public GameObject p2;
    public TextMeshProUGUI tmp; 

    void Update()
    {

        tmp.text = string.Format("{0}\n{1}", p1.transform.position.y, p2.transform.position.y); 

    }
}
