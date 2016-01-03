using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadDemoScene : MonoBehaviour
{

    public Slider loadingBar;
    public GameObject loadingImage;


    private AsyncOperation async;

    void Update()
    {
        if (Application.loadedLevel == 1)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                ClickAsync(2);
            }
        }
    }


    public void ClickAsync(int level)
    {
        loadingImage.SetActive(true);
        StartCoroutine(LoadLevelWithBar(level));
    }

    IEnumerator LoadLevelWithBar(int level)
    {
        async = Application.LoadLevelAsync(level);
        while (!async.isDone)
        {
            loadingBar.value = async.progress;
            yield return null;
        }
    }
}