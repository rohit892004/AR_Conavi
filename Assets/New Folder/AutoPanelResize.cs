using UnityEngine;
using TMPro;

public class AutoPanelResize : MonoBehaviour
{
    public RectTransform panelRect;
    public TMP_Text text;
    public float padding = 30f;

    void Update()
    {
        float textHeight = text.preferredHeight;
        panelRect.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            textHeight + padding
        );
    }
}
    