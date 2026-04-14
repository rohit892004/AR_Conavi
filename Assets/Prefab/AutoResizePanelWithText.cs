using UnityEngine;
using TMPro;

public class AutoResizePanelWithText : MonoBehaviour
{
    public RectTransform panelRect;
    public TextMeshProUGUI textTMP;

    public Vector2 padding = new Vector2(40f, 40f);

    void Update()
    {
        if (textTMP == null || panelRect == null) return;

        Vector2 textSize = textTMP.GetPreferredValues(
            textTMP.text,
            panelRect.rect.width,
            Mathf.Infinity
        );

        panelRect.sizeDelta = new Vector2(
            textSize.x + padding.x,
            textSize.y + padding.y
        );
    }
}
