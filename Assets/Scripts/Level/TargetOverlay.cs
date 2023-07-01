using UnityEngine;

public class TargetOverlay : MonoBehaviour
{
    private Transform target;
    private Canvas canvas;

    public void Show(Transform transform)
    {
        target = transform;
        canvas.enabled = true;
    }

    public void Hide()
    {
        target = null;
        canvas.enabled = false;
    }

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
    }
}
