using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    [SerializeField] Text timerText;
    [SerializeField] Text timesOverText; // Text to display "Time's Over"
    [SerializeField] float remainingTime;

    public bool TimerEnded { get; private set; } = false; // Public flag for other scripts

    AudioManager audioManager;
    private bool countdownStarted = false; // Flag to ensure countdown sound plays once

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        if (timesOverText != null)
        {
            timesOverText.gameObject.SetActive(false); // Ensure it's hidden at start
        }
    }

    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            remainingTime = Mathf.Max(remainingTime, 0);

            if (remainingTime <= 5f && !countdownStarted)
            {
                countdownStarted = true;
                audioManager.PlaySFX(audioManager.countdowntimer2); // Play the countdown SFX
            }

            if (remainingTime == 0 && !TimerEnded)
            {
                TimerEnded = true; // Mark timer as ended
                ShowTimesOverMessage(); // Display the "Time's Over" message
                LockGameInteraction(); // Lock game interactions
                StartCoroutine(DelayedSceneLoad(2f)); // Use coroutine for delay
            }
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void ShowTimesOverMessage()
    {
        if (timesOverText != null)
        {
            timesOverText.gameObject.SetActive(true); // Show the "Time's Over" text
            timesOverText.text = "Time's Up!";
        }
    }

    void LockGameInteraction()
    {
        // Stops all time-based interactions
        Time.timeScale = 0;
    }

    IEnumerator DelayedSceneLoad(float delay)
    {
        float timer = 0f;
        while (timer < delay)
        {
            timer += Time.unscaledDeltaTime; // Use unscaledDeltaTime to ignore Time.timeScale
            yield return null;
        }

        // Reset time scale before loading the next scene
        Time.timeScale = 1;
        SceneManager.LoadScene("Loading Screen"); // Replace with your scene name
    }
}
