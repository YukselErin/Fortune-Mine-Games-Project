using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    bool rouletteAlreadyLoaded = false;
    private GameObject RouletteSceneElementsRoot;
    [SerializeField] GameObject StartSceneElementsRoot;

    void Awake()
    {
        if (!Instance)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool IsRouletteAlreadyLoaded()
    {
        return rouletteAlreadyLoaded;
    }

    public void SetRouletteSceneRoot(GameObject gameObject)
    {
        RouletteSceneElementsRoot = gameObject;
        rouletteAlreadyLoaded = true;
    }
    public void ActivateRouletteSceneElements()
    {
        StartSceneElementsRoot.SetActive(false);
        if (rouletteAlreadyLoaded)
            RouletteSceneElementsRoot.SetActive(true);

    }
    public void ActivateMainScene()
    {
        StartSceneElementsRoot.SetActive(true);
        if (RouletteSceneElementsRoot)
            RouletteSceneElementsRoot.SetActive(false);
    }
}
