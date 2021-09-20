using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton that tracks and decays trippiness. Can be referenced anywhere
/// to apply the trip factor where necessary.
/// </summary>
public class TripManager : MonoBehaviour
{
    #region Singleton
    private static TripManager instance;
    public static TripManager Instance {  get { return instance; } }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    public float tripFactor = 0;

    private float decay = .01f;

    private void Decay(float deltaTime)
    {
        tripFactor = Mathf.Max(0, tripFactor - decay * deltaTime);
    }

    private void Update()
    {
        Decay(Time.deltaTime);
    }
}
