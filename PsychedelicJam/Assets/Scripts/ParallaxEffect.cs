using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parallax scrolling effect for background layers
/// </summary>
public class ParallaxEffect : MonoBehaviour
{
    private float length, startpos;

    public GameObject cam;
    public float parallaxFactor;

    private void Awake()
    {
        if (cam == null)
            cam = Camera.main.gameObject;
    }

    private void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float temp = cam.transform.position.x * (1 - parallaxFactor);
        float distance = cam.transform.position.x * parallaxFactor;
        Vector3 newPosition = new Vector3(startpos + distance, transform.position.y, transform.position.z);
        transform.position = newPosition;

        if (temp > startpos + (length / 2)) startpos += length;
        else if (temp < startpos - (length / 2)) startpos -= length;
    }
}
