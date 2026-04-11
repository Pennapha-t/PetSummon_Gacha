using UnityEngine;
using System;
using System.Collections;

public class ParryManager : MonoBehaviour
{
    public static ParryManager Instance;

    [Header("UI")]
    public GameObject parryButtonPrefab;
    public RectTransform canvasArea;

    [Header("Settings")]
    public int requiredSuccess = 3;
    public int maxFail = 3;

    public float nextSpawnDelay = 0.8f;
    public float spawnPadding = 120f;

    int successCount = 0;
    int failCount = 0;

    bool isRunning = false;
    bool waitingNext = false;

    Action<bool> resultCallback;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // ให้ BattleManager เช็คได้ว่า Parry กำลังทำงานอยู่ไหม
    public bool IsRunning()
    {
        return isRunning;
    }

    // เริ่มระบบ Parry
    public void StartParry(Action<bool> callback)
    {
        if (isRunning) return;

        successCount = 0;
        failCount = 0;

        resultCallback = callback;

        isRunning = true;
        waitingNext = false;

        ClearButtons();

        SpawnButton();
    }

    void SpawnButton()
    {
        if (!isRunning) return;

        GameObject btn = Instantiate(parryButtonPrefab, canvasArea);

        ParryButton pb = btn.GetComponent<ParryButton>();
        pb.Init(this);

        RectTransform rect = canvasArea;

        float x = UnityEngine.Random.Range(
            -rect.rect.width / 2 + spawnPadding,
             rect.rect.width / 2 - spawnPadding);

        float y = UnityEngine.Random.Range(
            -rect.rect.height / 2 + spawnPadding,
             rect.rect.height / 2 - spawnPadding);

        btn.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
    }

    public void OnSuccess()
    {
        if (!isRunning) return;

        successCount++;

        Debug.Log("Parry Success : " + successCount);

        CheckResult();
    }

    public void OnFail()
    {
        if (!isRunning) return;

        failCount++;

        Debug.Log("Parry Fail : " + failCount);

        CheckResult();
    }

    void CheckResult()
    {
        if (successCount >= requiredSuccess)
        {
            EndParry(true);
            return;
        }

        if (failCount >= maxFail)
        {
            EndParry(false);
            return;
        }

        if (!waitingNext)
        {
            StartCoroutine(SpawnNextButton());
        }
    }

    IEnumerator SpawnNextButton()
    {
        waitingNext = true;

        yield return new WaitForSeconds(nextSpawnDelay);

        SpawnButton();

        waitingNext = false;
    }

    void EndParry(bool result)
    {
        isRunning = false;

        ClearButtons();

        Debug.Log("Parry Result : " + result);

        resultCallback?.Invoke(result);
    }

    void ClearButtons()
    {
        foreach (Transform child in canvasArea)
        {
            Destroy(child.gameObject);
        }
    }
}