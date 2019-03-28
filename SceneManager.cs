using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour {
    public Text progressLoadingText;//отображение прогресса загрузки сцены
    public Slider progressLoadingSlider;

    public void SceneLoading(int indexScene)
    {
        StartCoroutine(StartSceneLoading(indexScene));
    }

    IEnumerator StartSceneLoading(int indexScene)
    {
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(indexScene);
        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress);
            progressLoadingSlider.value = progress;
            progressLoadingText.text = (progress * 100) + "%";
            yield return null;
        }
    }
}
