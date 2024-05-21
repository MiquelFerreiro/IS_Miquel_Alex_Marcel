using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Quaternion q;
    public bool manual;
    void Start()
    {
        if (GameObject.Find("PluginController").GetComponent<PluginConnector>().get_is_tracking_enabled()) {
            transform.position += new Vector3(0, 1.65f, 0);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setPosition(Vector3 pos)
    {
        //swith playerIndex
        transform.position = pos;
    }

    public void setRotation(Quaternion quat)
    {
        Matrix4x4 mat = Matrix4x4.Rotate(quat);
        transform.localRotation = quat;
    }
}
