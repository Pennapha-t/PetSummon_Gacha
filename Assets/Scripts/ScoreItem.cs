using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    public int scoreValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ชนกับ Player แล้ว!"); 
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.AddScore(scoreValue);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("หา ScoreManager ไม่เจอในฉาก!");
            }
        }
    }
}