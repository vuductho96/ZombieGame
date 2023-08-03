using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingBar : MonoBehaviour
{
    public Slider loadingSlider;
    public TextMeshProUGUI loadingTipText;
    public float tipDisplayTime = 3f; // Display each tip for 3 seconds
    public float chunkLoadingDelay = 1f; // Delay between loading chunks
    public string sceneToLoad = "MainGame";
    public string[] loadingTips;

    private int currentTipIndex = 0;
    private float targetProgress = 0f;
    private bool isLoadingChunks = false;

    private void Start()
    {
        loadingSlider.value = 0f;
        LoadSceneWithSlider();
    }

    private void LoadSceneWithSlider()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            ShowRandomLoadingTip(); // Show random loading tip

            if (!isLoadingChunks)
            {
                isLoadingChunks = true;
                StartCoroutine(LoadChunks(async));
            }

            yield return new WaitForSeconds(tipDisplayTime);
        }
    }

    private IEnumerator LoadChunks(AsyncOperation async)
    {
        while (loadingSlider.value < 1f)
        {
            while (loadingSlider.value < targetProgress)
            {
                loadingSlider.value += 0.01f; // Adjust the increment value as needed
                yield return new WaitForSeconds(chunkLoadingDelay);
            }

            targetProgress += 0.25f; // Adjust the target progress increment value as needed

            if (targetProgress > 1f)
            {
                targetProgress = 1f;
            }

            if (targetProgress == 1f)
            {
                async.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private void ShowRandomLoadingTip()
    {
        if (loadingTips.Length > 0 && loadingTipText != null)
        {
            int randomIndex = Random.Range(0, loadingTips.Length);
            loadingTipText.text = loadingTips[randomIndex];
        }
    }
}
