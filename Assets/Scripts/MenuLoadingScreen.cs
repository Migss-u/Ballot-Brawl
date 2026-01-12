using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLoadingScreen : MonoBehaviour
{
    public Text loadingText;

    private string[] loadingStates = { "Loading .", "Loading ..", "Loading ..." };
    private int currentStateIndex = 0;

    void Start()
    {
        StartCoroutine(LoadingSequence());
    }

    IEnumerator LoadingSequence()
    {
        float loadingDuration = 5f;
        float cycleInterval = 1f;

        float elapsedTime = 0f;

        while (elapsedTime < loadingDuration)
        {
            loadingText.text = loadingStates[currentStateIndex];
            currentStateIndex = (currentStateIndex + 1) % loadingStates.Length;
            yield return new WaitForSeconds(cycleInterval);
            elapsedTime += cycleInterval;
        }

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("Play Scene");
    }
}
