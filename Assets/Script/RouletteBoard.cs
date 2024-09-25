using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.AddressableAssets.GUI;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = UnityEngine.Random;

[System.Serializable]
public class WinRewardEvent : UnityEvent<string, Vector3, int>
{
}

public class RouletteBoard : MonoBehaviour
{
    public WinRewardEvent OnWinReward;
    public PlayerStats playerStats;
    [SerializeField] float roulettePeriod = 1.7f;
    [SerializeField] int rouletteSpinAmount = 10;
    [SerializeField] AssetPreset preset;
    [SerializeField] int columnAmount = 3;
    [SerializeField] float verticalMargin = 10f;
    [SerializeField] float horizontalMargin = 10f;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] int tileAmount = 12;
    [SerializeField] ReturnToMainScene returnToMainScene;
    [SerializeField] LoadSprites loadSprites;
    Dictionary<string, Sprite> rewardSprites;
    Sprite tileSprite;
    int currentlySpinning = 0;
    List<GameObject> activeTiles;
    List<GameObject> spentTiles;
    void Awake()
    {
        OnWinReward = new WinRewardEvent();
        activeTiles = new List<GameObject>();
        spentTiles = new List<GameObject>();
        rewardSprites = new Dictionary<string, Sprite>();
    }
    void Start()
    {
        GetSprites();
    }
    void GetSprites()
    {
        loadSprites.RequestSprites(preset, this);
    }
    public void UpdateSprites(Dictionary<string, Sprite> rewards, Sprite tile)
    {
        rewardSprites = rewards;
        tileSprite = tile;
        SetUpBoard();
    }
    public void SetUpBoard()
    {
        for (int i = 0; i < tileAmount; i++)
        {
            GameObject temp = CreateTile(transform.position + (i % columnAmount) * new Vector3(1f, 0f) * horizontalMargin + (i / columnAmount) * new Vector3(0, -1f) * verticalMargin);
            activeTiles.Add(temp);
        }
        PlaceRewards();
    }
    GameObject CreateTile(Vector2 position)
    {
        GameObject temp = Instantiate(tilePrefab, position, quaternion.identity, transform);
        temp.GetComponent<Tile>().SetMainTile(tileSprite);
        return temp;
    }
    void PlaceRewards()
    {
        currentlySpinning = 0;
        foreach (GameObject spentTile in spentTiles.ToArray())
        {
            activeTiles.Add(spentTile);
            spentTiles.Remove(spentTile);
        }
        foreach (GameObject activeTile in activeTiles)
        {
            activeTile.GetComponent<Tile>().SetMainTile(tileSprite);
            GetTileReward(activeTile);
        }
    }
    Sprite GetTile()
    {
        return tileSprite;
    }
    void GetTileReward(GameObject tile)
    {
        int index = Random.Range(0, rewardSprites.Count);
        KeyValuePair<string, Sprite> kvp = rewardSprites.ElementAt(index);
        tile.GetComponent<Tile>().SetRewardSprite(kvp.Value, kvp.Key);
    }
    public void SpinRoulette()
    {
        if (activeTiles.Count - currentlySpinning > 0)
        {
            currentlySpinning++;
            StartCoroutine(SpinSequence());

        }
    }
    IEnumerator SpinSequence()
    {
        int soFarSpin = 0;
        while (soFarSpin < rouletteSpinAmount - 1)
        {
            soFarSpin++;
            activeTiles[GetNextTileIndexRandom()].GetComponent<Tile>().HighlightTile();
            yield return new WaitForSeconds(roulettePeriod);
        }
        GameObject selected = activeTiles[GetNextTileIndexRandom()];
        selected.GetComponent<Tile>().SetRewardTaken();
        OnWinReward.Invoke(selected.GetComponent<Tile>().rewardAddress, selected.transform.position, 1);
        activeTiles.Remove(selected);
        spentTiles.Add(selected);
        currentlySpinning--;
        if (activeTiles.Count == 0)
        {
            Invoke("AllTilesSpentRetunToMain", .2f);
        }
    }
    int GetNextTileIndexRandom()
    {
        if (activeTiles.Count > 0)
        {
            return Random.Range(0, activeTiles.Count);
        }
        else
        {
            return -1;
        }
    }
    public Sprite GetSprite(string spriteAddress)
    {
        return rewardSprites[spriteAddress];
    }
    public void SetPlayerStats(PlayerStats newScenePlayerStats)
    {
        playerStats = newScenePlayerStats;
    }
    void OnEnable()
    {
        PlaceRewards();
    }
    void AllTilesSpentRetunToMain()
    {
        returnToMainScene.ActivateMainScene();
    }
}
