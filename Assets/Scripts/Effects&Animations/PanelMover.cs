using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

public class PanelMover : MonoBehaviour
{
    [SerializeField] private RectTransform panel; // Assign your UI panel in the Inspector
    [SerializeField] private Button closeButtonBackground; // Transparent full-screen button
    [SerializeField] private Vector2 hiddenPosition; // Where the panel starts (off-screen)
    [SerializeField] private Vector2 visiblePosition; // Where the panel moves to (on-screen)
    [SerializeField] private float moveDuration = 0.5f; // Time for the animation

    private bool isPanelVisible = false;

    void Start()
    {
        // Set the panel to the hidden position at the start
        panel.anchoredPosition = hiddenPosition;

    }

    public void TogglePanel()
    {
        isPanelVisible = !isPanelVisible;

        // Move to visible position if hidden, otherwise move back to hidden position
        panel.DOAnchorPos(isPanelVisible ? visiblePosition : hiddenPosition, moveDuration)
            .SetEase(Ease.OutQuad);
        //dis/activating background close button
        closeButtonBackground.gameObject.SetActive(isPanelVisible);



    }
}
