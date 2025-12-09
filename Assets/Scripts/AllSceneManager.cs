using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    public IEnumerator SelectAndLoad(int currentIndex)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(currentIndex, LoadSceneMode.Single);

        while (!loadOperation.isDone)
        {
            yield return null;
        }


    }





}
