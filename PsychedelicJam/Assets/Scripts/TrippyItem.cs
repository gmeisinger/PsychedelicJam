using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for items that increase trip factor.
/// </summary>
public class TrippyItem : MonoBehaviour
{
    public float tripFactor;
    public float floatFactor = 3f;
    public float floatSpeed = 3f;
    private float startHeight;
    
    private void Awake()
    {
        startHeight = transform.position.y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        TripManager.Instance.tripFactor += tripFactor;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        float offset = Mathf.Sin(Time.time * floatSpeed) * floatFactor;
        transform.position = new Vector3(transform.position.x, startHeight + offset);
    }
}
