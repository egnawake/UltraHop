using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private float rotationVelocityFactor = 1.0f;
    [SerializeField] private float maxPitchUpAngle;
    [SerializeField] private float minPitchDownAngle;
    [SerializeField] private float resetYawSpeed = 360.0f;
    [SerializeField] private float zoomAccelerationFactor = 1.0f;
    [SerializeField] private float zoomDeceleration;
    [SerializeField] private float minZoomDistance;
    [SerializeField] private float maxZoomDistance;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float deocclusionSpeed;
    [SerializeField] private float deocclusionBuffer;
    [SerializeField] private Transform occlusionPivot;

    private Transform cameraTransform;
    private float zoomAcceleration;
    private float zoomVelocity;
    private Vector3 deocclusionVector;
    private float zoomPosition;

    private void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        zoomVelocity = 0f;
        deocclusionVector = new Vector3(0, 0, deocclusionBuffer);
        zoomPosition = cameraTransform.localPosition.z;
    }

    private void Update()
    {
        UpdatePitch();
        UpdateYaw();
        UpdateZoom();
        PreventOcclusion();
    }

    private void UpdatePitch()
    {
        Vector3 rotation = transform.localEulerAngles;

        rotation.x += -Input.GetAxis("Mouse Y") * rotationVelocityFactor;

        if (rotation.x < 180)
            rotation.x = Mathf.Min(rotation.x, maxPitchUpAngle);
        else
            rotation.x = Mathf.Max(rotation.x, minPitchDownAngle);

        transform.localEulerAngles = rotation;
    }

    private void UpdateYaw()
    {
        Vector3 rotation = transform.localEulerAngles;

        rotation.y += Input.GetAxis("Mouse X") * rotationVelocityFactor;

        transform.localEulerAngles = rotation;
    }

    private void ResetYaw()
    {
        Vector3 rotation = transform.localEulerAngles;

        if (rotation.y != 0f)
        {
            if (rotation.y < 180.0f)
                rotation.y = Mathf.Max(rotation.y - resetYawSpeed * Time.deltaTime, 0f);
            else
                rotation.y = Mathf.Min(rotation.y + resetYawSpeed * Time.deltaTime, 360.0f);

            transform.localEulerAngles = rotation;
        }
    }

    private void UpdateZoom()
    {
        UpdateZoomAcceleration();
        UpdateZoomVelocity();
        UpdateZoomPosition();
    }

    private void UpdateZoomAcceleration()
    {
        zoomAcceleration = Input.GetAxis("Mouse ScrollWheel") * zoomAccelerationFactor;
    }

    private void UpdateZoomVelocity()
    {
        if (zoomAcceleration != 0f)
        {
            zoomVelocity += zoomAcceleration * Time.deltaTime;
        }
        else if (zoomVelocity > 0f)
        {
            zoomVelocity -= zoomDeceleration * Time.deltaTime;
            zoomVelocity = Mathf.Max(zoomVelocity, 0f);
        }
        else if (zoomVelocity < 0f)
        {
            zoomVelocity += zoomDeceleration * Time.deltaTime;
            zoomVelocity = Mathf.Min(0f, zoomVelocity);
        }
    }

    private void UpdateZoomPosition()
    {
        if (zoomVelocity != 0f)
        {
            Vector3 pos = cameraTransform.localPosition;

            pos.z += zoomVelocity * Time.deltaTime;

            if (pos.z < -maxZoomDistance)
            {
                pos.z = -maxZoomDistance;
                zoomVelocity = 0f;
            }
            else if (pos.z > -minZoomDistance)
            {
                pos.z = -minZoomDistance;
                zoomVelocity = 0;
            }

            cameraTransform.localPosition = pos;
            zoomPosition = pos.z;
        }
    }

    private void PreventOcclusion()
    {
        Vector3 rayDir = cameraTransform.position - playerTransform.position;

        if (Physics.Linecast(playerTransform.position,
            cameraTransform.position - cameraTransform.TransformDirection(deocclusionVector),
            out RaycastHit hit))
        {
            if (hit.collider.CompareTag("WorldBoundary"))
            {
                cameraTransform.position = hit.point +
                    cameraTransform.TransformDirection(deocclusionVector);
                cameraTransform.localPosition = new Vector3(0, 0, cameraTransform.localPosition.z);
            }
            else
            {
                Vector3 localPosition = cameraTransform.localPosition;
                localPosition.z += deocclusionSpeed * Time.deltaTime;
                cameraTransform.localPosition = localPosition;
            }
        }
        else
        {
            RevertDeocclusion();
        }
    }

    private void RevertDeocclusion()
    {
        Vector3 localPosition = cameraTransform.localPosition;

        if (localPosition.z > zoomPosition)
        {
            localPosition.z = Mathf.Max(localPosition.z - deocclusionSpeed * Time.deltaTime,
                zoomPosition);

            Vector3 worldPosition = transform.TransformPoint(localPosition);

            if (!Physics.Linecast(occlusionPivot.position,
                worldPosition - cameraTransform.TransformDirection(deocclusionVector)))
            {
                cameraTransform.localPosition = localPosition;
            }
        }
    }
}
