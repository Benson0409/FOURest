using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    //用於切換場景
    //由各個腳本來呼叫進行
    CanvasGroup canvasGroup;
    public float fadeInDuration;
    public float fadeOutDuration;


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        DontDestroyOnLoad(gameObject);
    }


    public IEnumerator loadFadeOutInScenes(string sceneName)
    {
        yield return FadeOut(fadeOutDuration);
        yield return loadScenes(sceneName);
        yield return FadeIn(fadeInDuration);
    }

    public IEnumerator FadeOut(float time)
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }

    public IEnumerator FadeIn(float time)
    {
        while (canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
        Destroy(gameObject);
    }

    public IEnumerator loadScenes(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        yield return null;
    }

   
}
