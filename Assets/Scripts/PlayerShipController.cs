using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    [Header("Movement")]
    public float thrustForce = 12f;
    public float strafeForce = 8f;
    public float rotationSpeed = 90f;
    public float maxSpeed = 12f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float strafeInput = Input.GetAxis("Horizontal");

        Vector3 force = transform.forward * forwardInput * thrustForce;
        force += transform.right * strafeInput * strafeForce;

        rb.AddForce(force, ForceMode.Force);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void Update()
    {
        float rotation = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            rotation = -1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotation = 1f;
        }

        transform.Rotate(Vector3.up, rotation * rotationSpeed * Time.deltaTime);
    }
}