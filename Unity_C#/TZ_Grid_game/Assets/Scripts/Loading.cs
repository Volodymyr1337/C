using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    private AsyncOperation LoadAsync;

    public Slider loadingBar;

    private void Start()
    {
        LoadAsync = Application.LoadLevelAsync(1);

        LoadAsync.allowSceneActivation = false;     // для имитации загрузки
    }
    
    private void Update()
    {
        if (LoadAsync.progress >= 0.9f)
            loadingBar.value += 0.01f;
        else
            loadingBar.value = LoadAsync.progress;

        if (loadingBar.value == 1f)
            LoadAsync.allowSceneActivation = true;
    }

}
