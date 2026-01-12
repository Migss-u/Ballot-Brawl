using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour
{
    public static MenuMusic instance;

    // Array of scene names where the menu music should not persist
    [SerializeField] private string[] scenesToMute = { "Menu Loading Screen", "Play Scene", "Loading Screen", "Result", "Death Scene", "Idle" };

    private AudioSource audioSource;
    private string previousScene;

    void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Initialize previous scene name
        previousScene = SceneManager.GetActiveScene().name;

        // Handle initial scene's music state
        HandleMusicForScene(previousScene);
    }

    void Update()
    {
        // Get the current scene name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Check if the scene has changed
        if (currentSceneName != previousScene)
        {
            previousScene = currentSceneName;

            // Handle music for the new scene
            HandleMusicForScene(currentSceneName);
        }
    }

    private void HandleMusicForScene(string sceneName)
    {
        // Check if the current scene is in the mute list
        if (System.Array.Exists(scenesToMute, scene => scene == sceneName))
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause(); // Pause the music
            }
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play(); // Resume the music
            }
        }
    }
}