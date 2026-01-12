using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitsGame : MonoBehaviour
{
    // Method to be called when the button is clicked
    public void ExitGame()
    {
        // Reset the high score
        if (PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.DeleteKey("HighScore");
        }

        // Save the changes
        PlayerPrefs.Save();

        // Exit the application
        Application.Quit();

        // Log for debugging purposes
        Debug.Log("Game is exiting, and HighScore has been reset.");
    }
}
