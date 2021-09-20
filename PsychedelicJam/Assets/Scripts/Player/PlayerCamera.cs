using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerTransform;
    //public SpriteRenderer background;
    [Range(0, .5f)] public float smoothTime = 0;

    private Vector3 velocity;
    // I'll be honest, I got this value through trial and error and I'm ashamed.
    private float bounds = 45;

    private void LateUpdate()
    {
        Vector3 target = new Vector3(playerTransform.position.x, Mathf.Clamp(playerTransform.position.y, -bounds, bounds), transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
    }
}
