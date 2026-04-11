using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("Settings")]
    public int catID; 
    public string catName;

    [Header("Stats")]
    public int maxHP = 100;
    public int currentHP;
    public int maxMP = 50;
    public int currentMP;

    [Header("UI References")]
    public StatBar hpBar; 
    public StatBar mpBar; 

    void Start()
    {
        currentHP = maxHP;
        currentMP = 0; 

        if (hpBar != null) hpBar.SetMaxStat(maxHP);
        if (mpBar != null)
        {
            mpBar.SetMaxStat(maxMP);
            mpBar.SetCurrentStat(currentMP);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (hpBar != null)
            hpBar.SetCurrentStat(currentHP);

        StartCoroutine(TakeDamageEffect());

        Debug.Log($"Player แมว ID: {catID} โดนโจมตี! เหลือ HP: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    // =============================
    // DAMAGE EFFECT (แดง + สั่น)
    // =============================
    IEnumerator TakeDamageEffect()
    {
        Image img = GetComponent<Image>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        Vector3 originalPos = transform.localPosition;

        Color originalColor =
            img != null ? img.color :
            sr != null ? sr.color :
            Color.white;

        // เปลี่ยนเป็นสีแดง
        if (img != null) img.color = Color.red;
        else if (sr != null) sr.color = Color.red;

        // สั่น
        for (int i = 0; i < 6; i++)
        {
            float x = Random.Range(-8f, 8f);
            float y = Random.Range(-8f, 8f);
            transform.localPosition = originalPos + new Vector3(x, y, 0);
            yield return new WaitForSeconds(0.02f);
        }

        transform.localPosition = originalPos;

        // ถ้ายังไม่ตายให้คืนสี
        if (currentHP > 0)
        {
            if (img != null) img.color = originalColor;
            else if (sr != null) sr.color = originalColor;
        }
    }

    // =============================
    // MP
    // =============================
    public void UseMP(int amount)
    {
        currentMP -= amount;
        currentMP = Mathf.Clamp(currentMP, 0, maxMP);
        if (mpBar != null) mpBar.SetCurrentStat(currentMP);
    }

    public void AddMP(int amount)
    {
        currentMP += amount;
        currentMP = Mathf.Clamp(currentMP, 0, maxMP);
        if (mpBar != null) mpBar.SetCurrentStat(currentMP);
    }

    public void Heal(float percent)
    {
        int healAmount = Mathf.RoundToInt(maxHP * percent);
        currentHP += healAmount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (hpBar != null)
            hpBar.SetCurrentStat(currentHP);

        Debug.Log($"{catName} ได้รับการฮีล {healAmount} HP");
    }

    // =============================
    // DIE
    // =============================
    void Die()
    {
        Debug.Log($"{catName} ตายแล้ว");

        // ปิด collider และ raycast
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null) col.enabled = false;

        Image img = GetComponent<Image>();
        if (img != null) img.raycastTarget = false;

        StopAllCoroutines(); // หยุดเอฟเฟกต์สั่นถ้ายังทำอยู่
        StartCoroutine(FadeOutAndDisable());
    }

    // =============================
    // FADE OUT EFFECT
    // =============================
    IEnumerator FadeOutAndDisable()
    {
        Image img = GetComponent<Image>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float duration = 0.5f;
        float t = 0f;

        Color startColor =
            img != null ? img.color :
            sr != null ? sr.color :
            Color.white;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / duration);

            if (img != null)
            {
                img.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            }
            else if (sr != null)
            {
                sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            }

            yield return null;
        }

        // ซ่อน object หลังจากจางหมด
        gameObject.SetActive(false);
    }
}