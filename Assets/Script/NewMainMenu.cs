using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGameScene()
    {
        StartCoroutine(AsyncLoadGameScene());
    }

    IEnumerator AsyncLoadGameScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("MainMap", LoadSceneMode.Single);
        while (!asyncOperation.isDone)
        {
            yield return null; // Wait until the scene finishes loading
        }
    }
}
