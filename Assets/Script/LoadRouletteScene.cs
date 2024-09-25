using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadRouletteScene : MonoBehaviour
{
    public void ActivateRouletteScene()
    {
        GameStateManager gameStateManager = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<GameStateManager>();
        gameStateManager.ActivateRouletteSceneElements();
        if (!gameStateManager.IsRouletteAlreadyLoaded())
        {
            StartCoroutine(LoadRouletteSceneAsync());

        }
    }
    IEnumerator LoadRouletteSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("RouletteScene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
