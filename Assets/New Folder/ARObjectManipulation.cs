using UnityEngine;

public class ARObjectManipulation : MonoBehaviour
{
    private float initialDistance;
    private Vector3 initialScale;

    public float rotationSpeed = 0.5f;
    public float scaleSpeed = 0.01f;
    public float minScale = 0.1f;
    public float maxScale = 2f;

    void Update()
    {
        // 🔄 ONE FINGER → ROTATE
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                float rotY = -touch.deltaPosition.x * rotationSpeed;
                transform.Rotate(0, rotY, 0);
            }
        }

        // 🔍 TWO FINGERS → SCALE (PINCH)
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touch0.position, touch1.position);
                initialScale = transform.localScale;
            }
            else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(touch0.position, touch1.position);

                if (Mathf.Approximately(initialDistance, 0))
                    return;

                float factor = currentDistance / initialDistance;
                Vector3 newScale = initialScale * factor;

                newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
                newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
                newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);

                transform.localScale = newScale;
            }
        }
    }
}
 