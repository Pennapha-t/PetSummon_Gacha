using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // ตั้งค่าเลือดเป็น 100
    public int currentHealth;
    public TMP_Text hpText;
    
    [Header("Next Character Settings")]
    public GameObject nextPlayerPrefab; // ลาก Prefab หรือ Object ตัวละครตัวที่ 2 มาใส่ตรงนี้

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHPUI(0);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        // พิมพ์ลง Console ให้เห็นชัดเจน
        Debug.Log("<color=red><b>[DAMAGE]</b></color> HP เหลือ: " + currentHealth);

        UpdateHPUI(damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHPUI(int lastDamage)
    {
        if (hpText != null)
            hpText.text = "HP: " + currentHealth;
    }

    // ส่วนหนึ่งของสคริปต์ PlayerHealth
void Die()
{
    Debug.Log("ตัวละครแรกตายแล้ว... กำลังส่งตัวละครถัดไปเข้าด่าน!");
    
    if (nextPlayerPrefab != null)
    {
        // เสก Player 2 ออกมา ณ ตำแหน่งที่ Player 1 ตาย
        Instantiate(nextPlayerPrefab, transform.position, transform.rotation);
    }

    Destroy(gameObject); // ทำลายศพ Player 1 ทิ้งไป
}
}