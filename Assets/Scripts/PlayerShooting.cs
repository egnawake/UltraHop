using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform weapon;
    [SerializeField] private Transform barrelEnd;
    [SerializeField] private float shootRange;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float shootRenderTime;
    [SerializeField] private TargetOverlay targetOverlay;

    private Transform cameraTransform;
    private LineRenderer lineRenderer;
    private Vector3 shootTarget;
    private Collider shootTargetCollider;
    private float currentShootCooldown;
    private float currentShootRenderTime;
    private PlayerHealth playerHealth;

    private void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        currentShootCooldown = shootCooldown;
        currentShootRenderTime = shootRenderTime;
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (!Input.GetMouseButton(1))
        {
            UpdateTarget();
            UpdateShoot();
        }

        UpdateTongue();
    }

    private void UpdateTarget()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward,
            out RaycastHit hitInfo, shootRange))
        {
            shootTarget = hitInfo.point;
            shootTargetCollider = hitInfo.collider;
            targetOverlay.Show(shootTargetCollider.transform);
        }
        else
        {
            shootTarget = cameraTransform.position + cameraTransform.forward *
                (shootRange + -cameraTransform.localPosition.z);
            targetOverlay.Hide();
        }
    }

    private void UpdateShoot()
    {
        currentShootCooldown = Mathf.Min(currentShootCooldown + Time.deltaTime,
            shootCooldown);

        currentShootRenderTime = Mathf.Min(currentShootRenderTime + Time.deltaTime,
            shootRenderTime);

        if (currentShootRenderTime == shootRenderTime)
            lineRenderer.enabled = false;

        if (currentShootCooldown == shootCooldown && Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        currentShootCooldown = 0f;
        currentShootRenderTime = 0f;

        if (shootTargetCollider != null)
        {
            Edible edible = shootTargetCollider.GetComponent<Edible>();
            if (edible != null)
            {
                edible.BeEaten();
                float regen = edible.HealthRegenerated;
                playerHealth.Heal(regen);
            }
        }

        lineRenderer.SetPosition(0, barrelEnd.position);
        lineRenderer.SetPosition(1, shootTarget);

        lineRenderer.enabled = true;
    }

    private void UpdateTongue()
    {
        if (!lineRenderer.enabled) return;

        lineRenderer.SetPosition(0, barrelEnd.position);

        float pct = Mathf.Sin((currentShootRenderTime / shootRenderTime) * Mathf.PI);
        Vector3 endPosition = Vector3.Lerp(barrelEnd.position,
            shootTarget, pct);

        lineRenderer.SetPosition(1, endPosition);
    }
}
