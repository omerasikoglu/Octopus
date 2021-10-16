using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject loadingInterface;
    public Image loadingProgressBar;

    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    public void StartGame()
    {
        HideMenu();
        ShowLoadingScreen();
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Gameplay"));
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Level1Part1", LoadSceneMode.Additive));
        //scenesToLoad.Add(SceneManager.LoadSceneAsync("Scene2", LoadSceneMode.Additive));
        StartCoroutine(LoadingScreen());
    }

    private void ShowLoadingScreen()
    {
        loadingInterface.SetActive(true);
    }

    private void HideMenu()
    {
        menu.SetActive(false);
    }

    IEnumerator LoadingScreen()
    {
        float totalProgress = 0;
        for (int i = 0; i < scenesToLoad.Count; ++i)
        {
            while (!scenesToLoad[i].isDone)
            {
                totalProgress += scenesToLoad[i].progress;
                loadingProgressBar.fillAmount = totalProgress / scenesToLoad.Count;
                yield return null;
            }
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
