using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VoteManager : MonoBehaviour
{
    // Vote count text fields for Player and Rivals
    public Text playerVoteText;
    public Text rivalVoteText1;
    public Text rivalVoteText2;
    public Text rivalVoteText3;

    // Vote counts
    public int playerVoteCount;
    private int rivalValue1 = 0;
    private int rivalValue2 = 0;
    private int rivalValue3 = 0;

    // Update interval for rival votes
    public float updateInterval = 1f;

    // Reference to GameTimer script
    private GameTimer gameTimer;

    void Start()
    {
        // Find and reference the GameTimer script
        gameTimer = FindObjectOfType<GameTimer>();

        // Start updating rival vote counts
        StartCoroutine(UpdateRivalVotes());
    }

    void Update()
    {
        // Update the vote counts on the UI
        UpdateVoteCounts();

        // Save the score and switch to the results scene when the game ends
        if (gameTimer != null && gameTimer.TimerEnded)
        {
            SaveVotes();
            SaveScores();
        }
    }

    System.Collections.IEnumerator UpdateRivalVotes()
    {
        yield return new WaitForSeconds(3f); // Initial delay before starting updates

        while (true)
        {
            // Stop updating if the timer has ended
            if (gameTimer != null && gameTimer.TimerEnded)
            {
                yield break; // Exit the coroutine
            }

            // Increment rival vote counts randomly
            rivalValue1 += Random.Range(1, 11);
            rivalValue2 += Random.Range(1, 11);
            rivalValue3 += Random.Range(1, 11);

            yield return new WaitForSeconds(updateInterval); // Wait before the next update
        }
    }

    void UpdateVoteCounts()
    {
        // Update the vote text fields
        if (playerVoteText != null)
        {
            playerVoteText.text = playerVoteCount.ToString();
        }

        if (rivalVoteText1 != null)
        {
            rivalVoteText1.text = rivalValue1.ToString();
        }

        if (rivalVoteText2 != null)
        {
            rivalVoteText2.text = rivalValue2.ToString();
        }

        if (rivalVoteText3 != null)
        {
            rivalVoteText3.text = rivalValue3.ToString();
        }
    }

    void SaveVotes()
    {
        // Save votes to PlayerPrefs
        PlayerPrefs.SetInt("PlayerVotes", playerVoteCount);
        PlayerPrefs.SetInt("Rival1Votes", rivalValue1);
        PlayerPrefs.SetInt("Rival2Votes", rivalValue2);
        PlayerPrefs.SetInt("Rival3Votes", rivalValue3);
        PlayerPrefs.Save(); // Ensure data is written immediately
    }


    void SaveScores()
    {
        // Save the current game score
        PlayerPrefs.SetInt("CurrentGameScore", playerVoteCount);

        // Check if a high score exists
        if (PlayerPrefs.HasKey("HighScore"))
        {
            int highScore = PlayerPrefs.GetInt("HighScore");

            // Compare the current score with the high score
            if (playerVoteCount > highScore)
            {
                PlayerPrefs.SetInt("HighScore", playerVoteCount); // Update the high score
            }
        }
        else
        {
            // Save the current score as the high score if no high score exists
            PlayerPrefs.SetInt("HighScore", playerVoteCount);
        }

        // Save the changes to PlayerPrefs
        PlayerPrefs.Save();
    }
}


