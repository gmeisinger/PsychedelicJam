using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public PlayerMovement player;
    public GameObject winPanel;
    public TimeKeeper time;
    public TMPro.TMP_Text scoreText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            SetScore();
            player.inputEnabled = false;
            winPanel.SetActive(true);
        }
    }

    private void SetScore()
    {
        int score = TripManager.Instance.score;
        int seconds = time.seconds;

        int finalScore = score - (seconds * 10);

        scoreText.text = "Final Score\n\n" + score.ToString() + " - " + seconds.ToString() + " seconds X 10 =\n\n" + finalScore.ToString() + " pts";
    }
}
