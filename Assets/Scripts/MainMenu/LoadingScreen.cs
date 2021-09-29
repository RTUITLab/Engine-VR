using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        yield return null;

        AsyncOperation async = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            slider.value = async.progress;
        }

        while (!async.isDone)
        {
            yield return null;
        }

        async.allowSceneActivation = true;
    }
}
