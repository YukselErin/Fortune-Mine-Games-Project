using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerRewardUIElement : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI amountDisplay;
    [SerializeField] SpriteRenderer spriteRenderer;
    public void AssignSprite(Sprite givenSprite)
    {
        spriteRenderer.sprite = givenSprite;
    }
    public void UpdateAmount(int amount)
    {
        amountDisplay.text = amount.ToString();
    }
    public void AnimateToTheSpot(bool firstOfItsKind, Vector3 from)
    {
        amountDisplay.enabled = false;
        Vector3 to = spriteRenderer.transform.position;
        spriteRenderer.transform.position = from;
        spriteRenderer.transform.DOJump(to, 1.5f, 2, 1).OnComplete(EnableAmountDisplay);
        spriteRenderer.transform.DOScale(new Vector3(.7f, .7f, .7f), 1).From();
    }
    void EnableAmountDisplay()
    {
        amountDisplay.enabled = true;
    }
}
