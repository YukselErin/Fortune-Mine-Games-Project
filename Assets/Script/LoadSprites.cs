using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadSprites : MonoBehaviour
{
    RouletteBoard rouletteBoard;
    List<AsyncOperationHandle<Sprite>> awaitingAddressables;
    List<AsyncOperationHandle<Sprite>> currentAddressableAssetHandles;
    Sprite tileSprite;
    Dictionary<string, Sprite> rewardSprites;
    AssetPreset preset;
    void Awake()
    {
        rewardSprites = new Dictionary<string, Sprite>();
        awaitingAddressables = new List<AsyncOperationHandle<Sprite>>();
    }
    public void RequestSprites(AssetPreset newpreset, RouletteBoard rboard)
    {
        preset = newpreset;
        rouletteBoard = rboard;
        foreach (string rewardAddress in preset.rewardItems)
        {
            StartCoroutine(GetSpriteAtAddress(rewardAddress, true));
        }
        StartCoroutine(GetSpriteAtAddress(preset.tileAddress, false));
        StartCoroutine(AwaitAllAddressables());
    }
    IEnumerator GetSpriteAtAddress(string address, bool isRewardSprite)
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(address);
        awaitingAddressables.Add(handle);
        while (!handle.IsDone)
        {
            yield return null;
        }
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            if (isRewardSprite)
            {
                rewardSprites.Add(address, handle.Result);
            }
            else
            {
                tileSprite = handle.Result;
            }
        }
    }
    IEnumerator AwaitAllAddressables()
    {
        bool allReturned = false;
        currentAddressableAssetHandles = new List<AsyncOperationHandle<Sprite>>();
        while (!allReturned)
        {
            foreach (AsyncOperationHandle<Sprite> handle in awaitingAddressables.ToArray())
            {
                if (handle.Status != AsyncOperationStatus.None)
                {
                    awaitingAddressables.Remove(handle);
                    currentAddressableAssetHandles.Add(handle);
                }
                if (awaitingAddressables.Count == 0)
                {
                    allReturned = true;
                }
            }
            yield return null;
        }
        rouletteBoard.UpdateSprites(rewardSprites, tileSprite);
    }
    private void OnDestroy()
    {
        ReleaseAllAddressables();
    }

    void ReleaseAllAddressables()
    {
        foreach (AsyncOperationHandle<Sprite> addressHandle in currentAddressableAssetHandles)
        {
            Addressables.Release(addressHandle);
        }
    }

}
