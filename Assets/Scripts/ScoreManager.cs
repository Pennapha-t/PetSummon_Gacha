using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int score = 0;
    public TMP_Text scoreText;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateUI();
        Debug.Log("แต้มเพิ่มเป็น: " + score); // เช็คใน Console
    }

    void UpdateUI()
    {
        if (scoreText != null)
    {
        // ต้องมีบรรทัดนี้เพื่อเอาตัวเลขจากตัวแปร score ไปโชว์บนจอ
        scoreText.text = "Score: " + score; 
    }
    
    }
}