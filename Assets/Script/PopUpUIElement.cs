using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;

public class PopUpUIElement : MonoBehaviour
{
    ObjectPool<GameObject> pool;
    [SerializeField] SpriteRenderer rewardSpriteRenderer;
    [SerializeField] float appearTime = .5f;
    [SerializeField] float waitTime = .5f;
    [SerializeField] float disappearTime = .2f;
    Vector3 fromPos;
    bool releaseMe = false;
    bool disabledOutside = false;

    public void SetPool(ObjectPool<GameObject> setPool)
    {
        pool = setPool;
    }
    public void HandleWinAward(Sprite sprite, Vector3 pos, int amount = 1)
    {
        rewardSpriteRenderer.sprite = sprite;
        transform.DOMove(pos, appearTime).OnComplete(Wait);
    }
    void Wait()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(WaitCoroutine());
        }
        else
        {
            releaseMe = true;
        }

    }
    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(waitTime);
        BackToStart();
    }
    void BackToStart()
    {
        transform.DOMove(fromPos, disappearTime).OnComplete(ReleaseToPool);
    }
    void ReleaseToPool()
    {
        if (!disabledOutside)
            pool.Release(gameObject);
    }
    public void SetFrom(Vector3 newFromPos)
    {
        fromPos = newFromPos;
    }
    void OnEnable()
    {
        transform.position = fromPos;
        disabledOutside = false;
        if (releaseMe)
        {
            releaseMe = false;
            ReleaseToPool();
        }

    }
    void OnDisable()
    {
        disabledOutside = true;
        ReleaseToPool();
    }
}
