using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetPreset", menuName = "ScriptableObjects/AssetPreset", order = 1)]

public class AssetPreset : ScriptableObject
{
   public string tileAddress;
   public string[] rewardItems;

}
