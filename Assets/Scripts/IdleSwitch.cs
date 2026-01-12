using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IdleSwitch : MonoBehaviour
{
    // The name of the scene to load after idle time
    [SerializeField]
    private string targetSceneName;

    // Idle time in seconds
    [SerializeField]
    private float idleTime = 60f;

    private float idleTimer;

    void Update()
    {
        // Check for mouse clicks or button presses
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2) || Input.anyKeyDown)
        {
            // Reset the idle timer if there is activity
            idleTimer = 0f;
        }
        else
        {
            // Increment the idle timer
            idleTimer += Time.deltaTime;
        }

        // Switch to the target scene if idle time exceeds the limit
        if (idleTimer >= idleTime)
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
