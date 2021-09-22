using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeKeeper : MonoBehaviour
{
    private float time = 0;
    [HideInInspector] public int seconds = 0;
    private TMP_Text txt;

    private void Awake()
    {
        txt = GetComponent<TMP_Text>();
    }

    void Update()
    {
        time += Time.deltaTime;
        seconds = (int)time;
        TimeSpan ts = TimeSpan.FromSeconds(seconds);
        txt.text = ts.ToString(@"mm\:ss");
    }
}
