using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void RestartCurrentScene()
    {
        StartCoroutine(AsyncRestartCurrentScene());
    }

    IEnumerator AsyncRestartCurrentScene()
    {
        // Reset the time scale to ensure game resumes normally
        Time.timeScale = 1f;

        // Get the name of the current active scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Single);
        while (!asyncOperation.isDone)
        {
            Time.timeScale = 1f;

            yield return null; // Wait until the scene finishes loading
        }
    }
}
