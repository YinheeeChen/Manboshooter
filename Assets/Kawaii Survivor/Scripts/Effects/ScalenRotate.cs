using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ScalenRotate : MonoBehaviour, IPointerDownHandler
{
    [Header("Elements")]
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);

        rectTransform.localScale = Vector2.one;

        LeanTween.scale(rectTransform, Vector2.one * 1.1f, 1)
            .setEase(LeanTweenType.punch)
            .setIgnoreTimeScale(true);

        rectTransform.rotation = Quaternion.identity;
        int sign = (int)Mathf.Sign(Random.Range(-1f, 1f));
        LeanTween.rotateAround(rectTransform, Vector3.forward, 15 * sign, 1)
            .setEase(LeanTweenType.punch)
            .setIgnoreTimeScale(true);

    }
}
