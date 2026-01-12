using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public float countdownTime = 3f;
    public Text countdownText;


    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        if (countdownText != null)
        {
            StartCoroutine(Countdown());
            audioManager.PlaySFX(audioManager.countdowntimer);

        }
    }

    IEnumerator Countdown()
    {
        Time.timeScale = 0f;

        float remainingTime = countdownTime;

        while (remainingTime > 0)
        {
            countdownText.text = remainingTime.ToString("F0");
            yield return new WaitForSecondsRealtime(1f);
            remainingTime -= 1f;
        }

        yield return new WaitForSecondsRealtime(0.2f);

        countdownText.text = "GO!!!";

        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1f;

        countdownText.gameObject.SetActive(false);
    }
}
