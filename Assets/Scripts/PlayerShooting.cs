using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform weapon;
    [SerializeField] private Transform barrelEnd;
    [SerializeField] private float shootRange;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float shootRenderTime;

    private Transform cameraTransform;
    private LineRenderer lineRenderer;
    private Vector3 shootTarget;
    private float currentShootCooldown;
    private float currentShootRenderTime;

    private void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        currentShootCooldown = shootCooldown;
        currentShootRenderTime = shootRenderTime;
    }

    private void Update()
    {
        if (!Input.GetMouseButton(1))
        {
            UpdateTarget();
            UpdateShoot();
        }
    }

    private void UpdateTarget()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward,
            out RaycastHit hitInfo))
        {
            shootTarget = hitInfo.point;
        }
        else
        {
            shootTarget = cameraTransform.position + cameraTransform.forward *
                (shootRange + -cameraTransform.localPosition.z);
        }

        weapon.LookAt(shootTarget);
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

        lineRenderer.SetPosition(0, barrelEnd.position);
        lineRenderer.SetPosition(1, shootTarget);

        lineRenderer.enabled = true;
    }
}
