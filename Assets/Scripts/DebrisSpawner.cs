using UnityEngine;

public class DebrisSpawner : MonoBehaviour
{
    public GameObject debrisPrefab;
    public Transform player;

    public int startAmount = 20;
    public float spawnRadius = 35f;
    public float minSize = 0.5f;
    public float maxSize = 2.5f;
    public float driftForce = 2f;

    void Start()
    {
        for (int i = 0; i < startAmount; i++)
        {
            SpawnDebris();
        }
    }

    public void SpawnDebris()
    {
        if (debrisPrefab == null || player == null) return;

        Vector3 randomDirection = Random.onUnitSphere;
        Vector3 spawnPosition = player.position + randomDirection * Random.Range(12f, spawnRadius);

        GameObject debris = Instantiate(debrisPrefab, spawnPosition, Random.rotation);

        float size = Random.Range(minSize, maxSize);
        debris.transform.localScale = Vector3.one * size;

        Rigidbody rb = debris.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.mass = size;
            rb.useGravity = false;

            Vector3 randomDrift = Random.onUnitSphere * driftForce;
            rb.AddForce(randomDrift, ForceMode.Impulse);
            rb.AddTorque(Random.onUnitSphere * driftForce, ForceMode.Impulse);
        }
    }
}