using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class Background : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private RenderTexture renderTexture;

    // Start is called before the first frame update
    void Start()
    {
        renderTexture = new RenderTexture(1920, 1080, 16, RenderTextureFormat.ARGB32);
        renderTexture.Create();

        videoPlayer.targetTexture = renderTexture;
        videoPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  

}
