using UnityEngine;
using TMPro;

public class PlayerMana : MonoBehaviour
{
    public float maxMana = 50f;
    public float currentMana;
    public float manaDecreaseRate = 0.5f; 
    public TMP_Text manaText;

    void Start()
    {
        currentMana = maxMana; // ตั้งค่าเริ่มต้นให้เต็ม 50
    }

    void Update()
    {
        if (currentMana > 0)
        {
            currentMana -= manaDecreaseRate * Time.deltaTime;
            
            // เพิ่มการพิมพ์ลง Console ทุกๆ 1 วินาที (เพื่อไม่ให้ Log รกเกินไป)
            if (Time.frameCount % 60 == 0) 
            {
                Debug.Log("<color=blue>มานาปัจจุบัน:</color> " + Mathf.FloorToInt(currentMana));
            }

            UpdateManaUI();
        }
    }

    void UpdateManaUI()
    {
        if (manaText != null)
            manaText.text = "MP: " + Mathf.FloorToInt(currentMana).ToString();
    }
}