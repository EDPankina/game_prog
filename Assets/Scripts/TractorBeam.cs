using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    [Header("Beam Settings")]
    public float captureRadius = 100f;
    public float moveSpeed = 12f;
    public Vector3 holdOffset = new Vector3(0f, 1.5f, 5f);
    public KeyCode beamKey = KeyCode.Space;

    [Header("Visual")]
    public LineRenderer lineRenderer;
    public Transform beamOrigin;

    private Rigidbody capturedRb;
    private bool previousKinematicState;

    private Collider[] shipColliders;
    private Collider[] capturedColliders;

    void Start()
    {
        if (beamOrigin == null)
        {
            beamOrigin = transform;
        }

        shipColliders = GetComponentsInChildren<Collider>();

        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(beamKey))
        {
            TryCaptureDebris();
        }

        if (Input.GetKeyUp(beamKey))
        {
            ReleaseDebris();
        }

        MoveCapturedDebris();
        UpdateBeamVisual();
    }

    void TryCaptureDebris()
    {
        if (capturedRb != null) return;

        GameObject[] debrisObjects = GameObject.FindGameObjectsWithTag("Debris");

        float closestDistance = captureRadius;
        Rigidbody closest = null;

        foreach (GameObject debris in debrisObjects)
        {
            float distance = Vector3.Distance(transform.position, debris.transform.position);

            if (distance < closestDistance)
            {
                Rigidbody rb = debris.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    closestDistance = distance;
                    closest = rb;
                }
            }
        }

        if (closest != null)
        {
            capturedRb = closest;

            previousKinematicState = capturedRb.isKinematic;

            capturedRb.velocity = Vector3.zero;
            capturedRb.angularVelocity = Vector3.zero;
            capturedRb.isKinematic = true;

            IgnoreShipCollision(true);
        }
    }

    void MoveCapturedDebris()
    {
        if (capturedRb == null) return;

        Vector3 targetPosition = beamOrigin.TransformPoint(holdOffset);

        capturedRb.transform.position = Vector3.Lerp(
            capturedRb.transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }

    void ReleaseDebris()
    {
        if (capturedRb != null)
        {
            IgnoreShipCollision(false);

            capturedRb.isKinematic = previousKinematicState;
            capturedRb = null;
        }

        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }

    void IgnoreShipCollision(bool ignore)
    {
        if (capturedRb == null) return;

        capturedColliders = capturedRb.GetComponentsInChildren<Collider>();

        foreach (Collider debrisCollider in capturedColliders)
        {
            foreach (Collider shipCollider in shipColliders)
            {
                if (debrisCollider != null && shipCollider != null)
                {
                    Physics.IgnoreCollision(debrisCollider, shipCollider, ignore);
                }
            }
        }
    }

    void UpdateBeamVisual()
    {
        if (lineRenderer == null) return;

        if (capturedRb != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, beamOrigin.position);
            lineRenderer.SetPosition(1, capturedRb.position);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}