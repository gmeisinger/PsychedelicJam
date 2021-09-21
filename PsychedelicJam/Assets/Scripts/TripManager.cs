using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System.Reflection;
using UnityEngine.Events;

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
    [HideInInspector] public int score = 0;
    [SerializeField] ForwardRendererData rendererData;
    public ScoreKeeper scoreKeeper;

    private float decay = .02f;
    private Dictionary<string, Blit> effects = new Dictionary<string, Blit>();

    private void Start()
    {
        //rendererData = GetRendererData();
        effects = rendererData.rendererFeatures.OfType<Blit>().ToDictionary(x => x.name, x => x);
    }

    private void Decay(float deltaTime)
    {
        tripFactor = Mathf.Clamp(tripFactor - decay * deltaTime, 0, 1);
    }

    private void Update()
    {
        Decay(Time.deltaTime);
        // update the materials
        effects["Wave"].settings.intensity = tripFactor * .5f;
        effects["Color"].settings.intensity = tripFactor * .5f;

        rendererData.SetDirty();
    }

    public void UpdateScore()
    {
        scoreKeeper.SetScore(score);
    }

    public void UpdateScore(int add)
    {
        score += (int)(add * (1 + tripFactor * 2));
        UpdateScore();
    }
}
