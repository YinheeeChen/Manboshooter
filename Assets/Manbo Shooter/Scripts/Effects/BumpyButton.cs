using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BumpyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("Elements")]
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!button.interactable)
        {
            return;
        }

        LeanTween.cancel(button.gameObject);
        LeanTween.scale(gameObject, new Vector2(1.1f, 0.9f), 0.6f)
            .setEase(LeanTweenType.easeOutElastic)
            .setIgnoreTimeScale(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable)
        {
            return;
        }

        LeanTween.cancel(button.gameObject);
        LeanTween.scale(gameObject, Vector2.one, 0.6f)
            .setEase(LeanTweenType.easeOutElastic)
            .setIgnoreTimeScale(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!button.interactable)
        {
            return;
        }

        LeanTween.cancel(button.gameObject);
        LeanTween.scale(gameObject, Vector2.one, 0.6f)
            .setEase(LeanTweenType.easeOutElastic)
            .setIgnoreTimeScale(true);
    }

}
