using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class DynamicCardSpacing : MonoBehaviour
{
    [Tooltip("Percentage of width to use for spacing (0–1).")]
    [Range(0f, 1f)]
    public float spacingPercent = 0.3f;

    HorizontalLayoutGroup _layout;
    RectTransform       _rt;

void Awake()
{
    _layout = GetComponent<HorizontalLayoutGroup>();
    _rt     = GetComponent<RectTransform>();
    // STOP the layout from ever resizing your cards
    _layout.childControlWidth     = false;
    _layout.childForceExpandWidth = false;
}


    void Start()
    {
        UpdateSpacing();
    }

    // This will also catch resolution changes in the editor or at runtime
    void OnRectTransformDimensionsChange()
    {
        UpdateSpacing();
    }

void UpdateSpacing()
{
    float referenceWidth = Screen.width;
    // positive = gap, negative = overlap
    float newSpacing = referenceWidth * spacingPercent;
    _layout.spacing = newSpacing;
    LayoutRebuilder.ForceRebuildLayoutImmediate(_rt);
}
}
