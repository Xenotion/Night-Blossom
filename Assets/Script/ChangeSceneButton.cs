using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    public void LoadGameScene()
    {
        Debug.Log("To menu");
        StartCoroutine(AsyncLoadGameScene());
    }

    IEnumerator AsyncLoadGameScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("NewMainMenu", LoadSceneMode.Single);
        while (!asyncOperation.isDone)
        {
            Time.timeScale = 1f;

            yield return null; // Wait until the scene finishes loading
        }
    }
}
