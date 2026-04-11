using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaleEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 originalScale;
    public float scaleMultiplier = 1.2f;
    public float speed = 10f;

    private Vector3 targetScale;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetScale = originalScale * scaleMultiplier;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
}