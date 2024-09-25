using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HeadBobTween : MonoBehaviour
{
    [SerializeField] float x = 0;
    [SerializeField] float y = 0;
    [SerializeField] float z = 0;
    [SerializeField] float magnitude = 1;
    [SerializeField] float timing = 1;
    void OnEnable()
    {
        rotateVec = new Vector3(x, y, z);
        StartCoroutine(AnimateBounce());
    }
    Vector3 rotateVec;
    IEnumerator AnimateBounce()
    {
        float maxScale = transform.localScale.y + .015f;
        float minScale = transform.localScale.y - .015f;
        while (enabled)
        {
            transform.DORotate(rotateVec * magnitude, timing);
            yield return new WaitForSeconds(timing);
            transform.DORotate(-1 * rotateVec * magnitude, timing);
            yield return new WaitForSeconds(timing);


        }
    }
}
