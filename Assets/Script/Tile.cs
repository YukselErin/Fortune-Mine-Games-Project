using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    public string rewardAddress;
    [SerializeField] float highlightTime = .1f;
    [SerializeField] SpriteRenderer mainTile;
    [SerializeField] SpriteRenderer highlightTile;
    [SerializeField] SpriteRenderer rewardSprite;
    [SerializeField] Sprite rewardTakenSprite;
    [SerializeField] Sprite rewardTakenTile;
    [SerializeField] bool rewardTaken = false;
    [SerializeField] int selectionFlashAmount = 4;
    [SerializeField] float selectionFlashPeriod = .7f;
    Vector3 startScale;
    void Awake()
    {
        startScale = rewardSprite.transform.localScale;
    }
    public void SetMainTile(Sprite tileSprite)
    {
        mainTile.sprite = tileSprite;
    }
    public void SetHighlightTile(Sprite tileSprite)
    {
        highlightTile.sprite = tileSprite;
    }
    public void SetRewardSprite(Sprite newRewardSprite, string address)
    {
        rewardAddress = address;
        rewardSprite.sprite = newRewardSprite;
    }

    public void HighlightTile()
    {
        highlightTile.DOFade(1f, highlightTime / 2).OnComplete(FadeOutHighLight);
    }
    public void FadeOutHighLight()
    {
        highlightTile.DOFade(0f, highlightTime / 2);
    }
    public void SetRewardTaken()
    {
        StartCoroutine(FlashHighlight(selectionFlashAmount, selectionFlashPeriod));
        rewardTaken = true;
        rewardSprite.sprite = rewardTakenSprite;
        mainTile.sprite = rewardTakenTile;
    }

    IEnumerator FlashHighlight(int times, float period)
    {
        Color temp = highlightTile.color;

        for (int i = 0; i < times; i++)
        {
            temp.a = 1;
            highlightTile.color = temp;
            yield return new WaitForSeconds(period);
            temp.a = 0;
            highlightTile.color = temp;
            yield return new WaitForSeconds(period);
        }
    }
    void OnEnable()
    {
        rewardSprite.transform.localScale = startScale;
        Color temp = highlightTile.color;
        temp.a = 0f;
        highlightTile.color = temp;
        rewardTaken = false;
        StartCoroutine(AnimateBounce());
    }
    IEnumerator AnimateBounce()
    {
        float maxScale = rewardSprite.transform.localScale.y + .015f;
        float minScale = rewardSprite.transform.localScale.y - .015f;
        while (enabled && !rewardTaken)
        {
            rewardSprite.transform.DOScaleY(maxScale, 1f);
            yield return new WaitForSeconds(1f);
            rewardSprite.transform.DOScaleY(minScale, 1f);
            yield return new WaitForSeconds(1f);
        }
    }
}
