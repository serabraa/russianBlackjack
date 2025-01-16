using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class CardUIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector2 originalScale;
    private Image cardImage;
    private Color originalColor;
    [SerializeField] private float hoverScale = 1.2f; // Scale factor on hover
    // [SerializeField] private Color hoverColor = Color.gray; // Outline or border color (optional)ß


    void Start()
    {
        originalScale = transform.localScale;
        cardImage = GetComponent<Image>();
        originalColor = cardImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * hoverScale;
        // cardImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        // cardImage.color = originalColor;
    }
}
