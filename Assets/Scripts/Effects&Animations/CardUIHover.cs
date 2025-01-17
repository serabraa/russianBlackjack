using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardUIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector2 originalScale;
    private Image cardImage;
    private Color originalColor;
    [SerializeField] private float hoverScale = 1.2f; // Scale factor on hover
    [SerializeField] private float hoverDuration = 0.2f; // Animation speed
    [SerializeField] private Color hoverColor = Color.gray; // Outline or border color (optional)


    void Start()
    {
        originalScale = transform.localScale;
        cardImage = GetComponent<Image>();
        originalColor = cardImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Smoothly increase the size
        transform.DOScale(originalScale * hoverScale, hoverDuration);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Smoothly return to original size
        transform.DOScale(originalScale, hoverDuration);

    }
}
