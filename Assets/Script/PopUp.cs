using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

public class PopUp : MonoBehaviour
{
    [SerializeField] RouletteBoard rouletteBoard;
    [SerializeField] Transform PopUpFrom;
    [SerializeField] Transform PopUpTo;
    [SerializeField] PopUpSpawner popUpSpawner;
    [SerializeField] float horizontalSpacing = .3f;
    void Start()
    {
        rouletteBoard.OnWinReward.AddListener(HandleWinAward);
    }
    void HandleWinAward(string spriteAddress, Vector3 pos, int amount = 1)
    {
        GameObject UIElement = popUpSpawner.pool.Get();
        Vector3 toPos = PopUpTo.position - new Vector3(0f, horizontalSpacing, 0f) * (popUpSpawner.pool.CountActive - 1);
        UIElement.transform.position = PopUpFrom.position;
        UIElement.GetComponent<PopUpUIElement>().SetFrom(PopUpFrom.position);
        UIElement.GetComponent<PopUpUIElement>().HandleWinAward(rouletteBoard.GetSprite(spriteAddress), toPos, 1);
    }

}
