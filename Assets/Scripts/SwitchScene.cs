using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DelayedSceneSwitch());
    }

    private IEnumerator DelayedSceneSwitch()
    {
        yield return new WaitForSeconds(5f); 
        SceneManager.LoadScene("Main Menu"); 
    }
}
