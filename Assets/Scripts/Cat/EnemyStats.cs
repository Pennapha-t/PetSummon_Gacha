using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyStats : MonoBehaviour
{
    [Header("Settings")]
    public int catID;
    public int level = 1;

    [Header("Stats")]
    public int maxHP = 100;
    public int currentHP;

    [Header("Enrage System")]
    public int hitCount = 0;
    public float damageMultiplier = 1.0f;
    public bool isEnraged = false;

    [Header("UI References")]
    public StatBar hpBar;

    void Start()
    {
        currentHP = maxHP;

        if (hpBar != null)
            hpBar.SetMaxStat(maxHP);
    }

    // =========================
    // รับดาเมจ
    // =========================
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (hpBar != null)
            hpBar.SetCurrentStat(currentHP);

        hitCount++;

        // โกรธเมื่อโดนตี 2 ครั้ง
        if (hitCount >= 2 && !isEnraged)
        {
            SetEnrage(true);
        }

        StartCoroutine(TakeDamageEffect());

        Debug.Log($"Enemy ID:{catID} Hit:{hitCount}/2 Damage:{damage} HP:{currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    // =========================
    // Enrage
    // =========================
    public void SetEnrage(bool enrage)
    {
        isEnraged = enrage;

        Image img = GetComponent<Image>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (enrage)
        {
            damageMultiplier = 2f;

            Color orange = new Color(1f, 0.5f, 0f);

            if (img != null) img.color = orange;
            else if (sr != null) sr.color = orange;

            Debug.Log($"Enemy {catID} ENRAGED! Damage x2");
        }
        else
        {
            damageMultiplier = 1f;
            hitCount = 0;

            if (img != null) img.color = Color.white;
            else if (sr != null) sr.color = Color.white;
        }
    }

    // =========================
    // เอฟเฟกต์โดนโจมตี
    // =========================
    IEnumerator TakeDamageEffect()
    {
        Image img = GetComponent<Image>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        Vector3 originalPos = transform.localPosition;

        for (int i = 0; i < 8; i++)
        {
            transform.localPosition = originalPos + new Vector3(Random.Range(-10, 10), 0, 0);
            yield return new WaitForSeconds(0.02f);
        }
        transform.localPosition = originalPos;

        if (isEnraged)
        {
            Color orange = new Color(1f, 0.5f, 0f);

            if (img != null) img.color = orange;
            else if (sr != null) sr.color = orange;
        }
        else
        {
            if (img != null) img.color = Color.white;
            else if (sr != null) sr.color = Color.white;
        }
    }

    // =========================
    // ตาย
    // =========================
    void Die()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null)
            col.enabled = false;

        // แจ้ง BattleManager
        if (BattleManager.Instance != null)
            BattleManager.Instance.OnEnemyDied(this);

        StopAllCoroutines();
        StartCoroutine(EnemyDieEffect());
    }

    IEnumerator EnemyDieEffect()
    {
        Image img = GetComponent<Image>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float alpha = 1f;

        for (int i = 0; i < 8; i++)
        {
            transform.localPosition += new Vector3(Random.Range(-10, 10), 0, 0);
            yield return new WaitForSeconds(0.02f);
        }

        while (alpha > 0)
        {
            alpha -= Time.deltaTime * 2f;

            if (img != null)
            {
                Color c = img.color;
                c.a = alpha;
                img.color = c;
            }

            if (sr != null)
            {
                Color c = sr.color;
                c.a = alpha;
                sr.color = c;
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }
}