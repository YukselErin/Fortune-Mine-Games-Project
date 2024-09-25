using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainScene : MonoBehaviour
{
    [SerializeField] GameObject rouletteSceneRoot;
    public void ActivateMainScene()
    {
        GameObject stateManagerGO = GameObject.FindGameObjectWithTag("GameStateManager");
        if (stateManagerGO)
        {
            GameStateManager gameStateManager = stateManagerGO.GetComponent<GameStateManager>();
            if (gameStateManager)
            {
                gameStateManager.SetRouletteSceneRoot(rouletteSceneRoot);

                gameStateManager.ActivateMainScene();
            }

        }
        //StartCoroutine(LoadMainAsync());
    }
    public IEnumerator LoadMainAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("StartScene");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
