using UnityEngine;

public class ResourceBar : MonoBehaviour
{
    [SerializeField] private RectTransform barTransform;

    private float barMaxSize;

    public void SetFill(float percentage)
    {
       float size = percentage * barMaxSize;

       barTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
    }

    private void Awake()
    {
        barMaxSize = barTransform.rect.width;
    }
}
