using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToOptions: MonoBehaviour
{
    public void LoadGameScene()
    {
        StartCoroutine(AsyncLoadGameScene());
    }

    IEnumerator AsyncLoadGameScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("OptionScene", LoadSceneMode.Single);
        while (!asyncOperation.isDone)
        {
            Time.timeScale = 1f;

            yield return null; // Wait until the scene finishes loading
        }
    }
}
