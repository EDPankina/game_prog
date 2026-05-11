using UnityEngine;

public class RecycleZone : MonoBehaviour
{
    public int scorePerDebris = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Debris"))
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(scorePerDebris);
            }

            Destroy(other.gameObject);
        }
    }
}