using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    private TMP_Text txt;
    private string fmt = "00000.##";

    private void Awake()
    {
        txt = GetComponent<TMP_Text>();
    }

    public void SetScore(int score)
    {
        txt.text = score.ToString(fmt);
    }
}
