using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Text loadingText;

    private string[] loadingStates = { "Loading .", "Loading ..", "Loading ..." };
    private string[] finalMessages = { "Cleaning Up", "Tallying Result" };
    private int currentStateIndex = 0;

    void Start()
    {
        // Start the coroutine to handle text changes
        StartCoroutine(LoadingSequence());
    }

    IEnumerator LoadingSequence()
    {
        // Cycle through "Loading ." states
        for (int i = 0; i < 9; i++) // 3 cycles of "Loading .", "Loading ..", "Loading ..."
        {
            loadingText.text = loadingStates[currentStateIndex];
            currentStateIndex = (currentStateIndex + 1) % loadingStates.Length;
            yield return new WaitForSeconds(1f); // Change every second
        }

        // Show final messages in sequence
        foreach (string message in finalMessages)
        {
            loadingText.text = message;
            yield return new WaitForSeconds(3f); // Show each message for 3 seconds
        }

        // Wait for an additional 3 seconds before loading the "Result" scene
        yield return new WaitForSeconds(3f);

        // Load the "Result" scene
        SceneManager.LoadScene("Result");
    }
}
