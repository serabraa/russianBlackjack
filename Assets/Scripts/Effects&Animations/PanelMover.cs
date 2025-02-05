using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

public class PanelMover : MonoBehaviour
{
    [SerializeField] private RectTransform DeckPanel; // Assigned UI DeckPanel in the Inspector
    [SerializeField] private RectTransform ShopPanel;
    [SerializeField] private Button closeButtonBackground; // Transparent full-screen button
    [SerializeField] private Vector2 hiddenPosition; // Where the DeckPanel starts (off-screen)
    [SerializeField] private Vector2 visiblePosition; // Where the DeckPanel moves to (on-screen)
    [SerializeField] private float moveDuration = 0.5f; // Time for the animation

    [SerializeField] private Vector2 hiddenShopPosition;
    [SerializeField] private Vector2 visibleShopPosition;
    private bool isShopPanelVisible = false;
    private bool isBossDeckPanelVisible = false;            //bool if is visible

    private bool isDeckPanelVisible = false;

    void Start()
    {
        // Set the DeckPanel to the hidden position at the start
        DeckPanel.anchoredPosition = hiddenPosition;
        // Set the ShopPanel to the hidden position at the start
        ShopPanel.anchoredPosition = hiddenShopPosition;

    }

    public void ToggleDeckPanel()
    {
        isDeckPanelVisible = !isDeckPanelVisible;

        // Move to visible position if hidden, otherwise move back to hidden position
        DeckPanel.DOAnchorPos(isDeckPanelVisible ? visiblePosition : hiddenPosition, moveDuration)
            .SetEase(Ease.OutQuad);
        //dis/activating background close button
        closeButtonBackground.gameObject.SetActive(isDeckPanelVisible);
    }

        public void TogglShopPanel()
    {
        isShopPanelVisible = !isShopPanelVisible;
        // Move to visible position if hidden, otherwise move back to hidden position
        ShopPanel.DOAnchorPos(isShopPanelVisible ? visibleShopPosition : hiddenShopPosition, moveDuration)
            .SetEase(Ease.OutQuad);
    }
}
