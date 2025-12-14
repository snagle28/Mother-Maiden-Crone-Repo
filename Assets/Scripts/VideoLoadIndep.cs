using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoLoadIndep : MonoBehaviour
{
    private float videoTimer = 0f;

    void Update()
    {
        videoTimer += Time.deltaTime;

        if(videoTimer >= 35f)
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {

        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);


    }





}
