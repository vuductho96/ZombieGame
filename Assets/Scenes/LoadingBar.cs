using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBar : MonoBehaviour
{
    public Slider LoadingSlide;
    public Text[] Tips;

    public void RanDomTips()
    {
        // Random tips implementation
    }

    public void AccurateLoadingScene()
    {
        StartCoroutine(LoadMainGameScene());
    }

    private IEnumerator LoadMainGameScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("MainGame");
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f); // Normalize the progress value between 0 and 1
            LoadingSlide.value = progress;

            if (progress >= 1f)
            {
                async.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
