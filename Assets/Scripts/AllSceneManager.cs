
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class AllSceneManager : MonoBehaviour
{
    private int currentSceneIndex;

    public void LoadSceneSubsequent()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        currentSceneIndex = currentScene.buildIndex;

        currentSceneIndex += 1;

        StartCoroutine(SelectAndLoad(currentSceneIndex));
    }

    public void LoadStartScene()
    {
        StartCoroutine(SelectAndLoad("StartScene"));
    }

    public IEnumerator SelectAndLoad(int currentIndex)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(currentIndex, LoadSceneMode.Single);

        while (!loadOperation.isDone)
        {
            yield return null;
        }
    }

    public IEnumerator SelectAndLoad(string sceneName)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!loadOperation.isDone)
        {
            yield return null;
        }
    }
}

