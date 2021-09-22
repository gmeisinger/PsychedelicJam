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
    public AudioSource music;

    private float decay = .02f;
    private Dictionary<string, Blit> effects = new Dictionary<string, Blit>();

    private void Start()
    {
        //rendererData = GetRendererData();
        effects = rendererData.rendererFeatures.OfType<Blit>().ToDictionary(x => x.name, x => x);
        StartCoroutine(IntroWarp());
    }

    private void Decay(float deltaTime)
    {
        tripFactor = Mathf.Clamp(tripFactor - decay * deltaTime, 0, 1);
    }

    private void Update()
    {
        Decay(Time.deltaTime);
        // update the materials
        effects["Wave"].settings.intensity = tripFactor * .2f;
        effects["Color"].settings.intensity = tripFactor * .5f;

        rendererData.SetDirty();

        // update the music
        UpdateMusic();
    }

    public void UpdateScore()
    {
        if (scoreKeeper == null) return;
        scoreKeeper.SetScore(score);
    }

    private void UpdateMusic()
    {
        if (music == null) return;
        music.pitch = 1 + tripFactor * .5f;
    }

    public void UpdateScore(int add)
    {
        score += (int)(add * (1 + tripFactor * 2));
        UpdateScore();
    }

    private IEnumerator IntroWarp()
    {
        float duration = .5f;
        float timer = 0;
        float intensity = 1;
        while(intensity > 0)
        {
            effects["Wave"].settings.intensity = intensity;
            effects["Color"].settings.intensity = intensity;
            intensity = Mathf.Lerp(1, 0, timer/duration);
            timer += Time.deltaTime;
            yield return null;
        }
        effects["Wave"].settings.intensity = 0;
        effects["Color"].settings.intensity = 0;
    }

    private void OnDisable()
    {
        effects["Wave"].settings.intensity = 0;
        effects["Color"].settings.intensity = 0;
    }
}
