using UnityEngine;
using UnityEngine.UI;

public class AdjustPanelSize : MonoBehaviour
{
    public Text textComponent;
    public RectTransform panelRectTransform;
    public Vector2 padding = new Vector2(20f, 20f); // Padding to add around the text

    void Update()
    {
        // Get the preferred width and height of the text
        float preferredWidth = textComponent.preferredWidth + padding.x;
        float preferredHeight = textComponent.preferredHeight + padding.y;

        // Set the size of the panel based on the preferred width and height of the text
        panelRectTransform.sizeDelta = new Vector2(preferredWidth, preferredHeight);
    }
}
