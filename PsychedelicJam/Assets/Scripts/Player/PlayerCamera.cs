using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerTransform;
    [Range(0, .5f)] public float smoothTime = 0;
    //[Range(0, 180)] public int zoom = 0;

    private Vector3 velocity;
    //private PixelPerfectCamera ppCam;
    //private int defaultResolutionX;
    //private int defaultResolutionY;

    //private void Awake()
    //{
    //    ppCam = GetComponent<PixelPerfectCamera>();
    //    defaultResolutionX = ppCam.refResolutionX;
    //    defaultResolutionY = ppCam.refResolutionY;
    //}

    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z), ref velocity, smoothTime);
        //ppCam.refResolutionX = defaultResolutionX - zoom * 2;
        //ppCam.refResolutionY = defaultResolutionY - zoom;
    }
}
