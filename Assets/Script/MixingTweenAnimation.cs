using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MixingTweenAnimation : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(AnimateBounce());
    }
    public float magnitude = 5f;
    public float timing = 2f;
    public int vibrato = 10;
    public float elasticity = 1f;
    IEnumerator AnimateBounce()
    {
        float maxScale = transform.localScale.y + .015f;
        float minScale = transform.localScale.y - .015f;
        while (enabled)
        {
            transform.DOPunchRotation(Vector3.back * magnitude, timing, vibrato, elasticity);
            yield return new WaitForSeconds(1f);
            transform.DOPunchRotation(Vector3.forward * magnitude, timing, vibrato, elasticity);
            yield return new WaitForSeconds(1f);
        }
    }
}
