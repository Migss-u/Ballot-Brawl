using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsManager : MonoBehaviour
{
    // Text fields to display scores
    public Text lastGameScoreText;
    public Text currentGameScoreText;

    void Start()
    {
        // Retrieve and display the high score (last game score)
        if (PlayerPrefs.HasKey("HighScore"))
        {
            int highScore = PlayerPrefs.GetInt("HighScore");
            lastGameScoreText.text = highScore.ToString();
        }
        else
        {
            lastGameScoreText.text = "0"; // Default to 0 if no high score exists
        }

        // Retrieve and display the current game score
        if (PlayerPrefs.HasKey("CurrentGameScore"))
        {
            int currentScore = PlayerPrefs.GetInt("CurrentGameScore");
            currentGameScoreText.text = currentScore.ToString();
        }
        else
        {
            currentGameScoreText.text = "0"; // Default to 0 if no current game score exists
        }
    }
}
