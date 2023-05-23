using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ParallaxEffect : MonoBehaviour
{

    private float startposX, startposY;

    public GameObject cam;
    public float parallaxEffect;

  
    void Start()
    {
        startposX = transform.position.x;
        startposY = transform.position.y;
      
    }

    void FixedUpdate()
    {
        float distX = (cam.transform.position.x * parallaxEffect);
        float distY = (cam.transform.position.y * parallaxEffect) -2;
        transform.position = new Vector3(startposX + distX, startposY + distY, transform.position.z);
    }

   
}
