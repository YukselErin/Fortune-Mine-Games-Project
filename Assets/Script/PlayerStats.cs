using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] GameObject statDisplayPrefab;
    [SerializeField] GameObject Canvas;
    Dictionary<string, int> gainedRewardAmount;
    Dictionary<string, GameObject> gainedRewardUIElements;
    [SerializeField] RouletteBoard rouletteBoard;
    [SerializeField] float rewardUIVerticalSpace = 1f;
    [SerializeField] float rewardUIHorizontalSpace = 1f;
    void Start()
    {
        gainedRewardAmount = new Dictionary<string, int>();
        gainedRewardUIElements = new Dictionary<string, GameObject>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        NewBoard();
    }
    public void NewBoard()
    {
        GameObject rouletteBoardGO = GameObject.FindWithTag("RouletteBoard");
        if (rouletteBoardGO)
        {
            rouletteBoard = rouletteBoardGO.GetComponent<RouletteBoard>();
            rouletteBoard.SetPlayerStats(this);
            rouletteBoard.OnWinReward.AddListener(AwardReward);
        }
    }
    public void AwardReward(string rewardedName, Vector3 from, int amount = 1)
    {
        if (gainedRewardAmount.ContainsKey(rewardedName))
        {
            gainedRewardAmount[rewardedName] += amount;
            gainedRewardUIElements[rewardedName].GetComponent<PlayerRewardUIElement>().UpdateAmount(gainedRewardAmount[rewardedName]);
        }
        else
        {
            gainedRewardAmount.Add(rewardedName, amount);
            Vector3 instantiatePosition = transform.position + new Vector3(rewardUIVerticalSpace * (gainedRewardUIElements.Count % 7), -1f * rewardUIHorizontalSpace * (gainedRewardUIElements.Count / 7));
            gainedRewardUIElements.Add(rewardedName, Instantiate(statDisplayPrefab, instantiatePosition, quaternion.identity, Canvas.transform));
            gainedRewardUIElements[rewardedName].GetComponent<PlayerRewardUIElement>().AssignSprite(rouletteBoard.GetSprite(rewardedName));
            gainedRewardUIElements[rewardedName].GetComponent<PlayerRewardUIElement>().UpdateAmount(gainedRewardAmount[rewardedName]);
            gainedRewardUIElements[rewardedName].GetComponent<PlayerRewardUIElement>().AnimateToTheSpot(true, from);
        }
    }
}
