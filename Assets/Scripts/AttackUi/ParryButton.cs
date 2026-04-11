using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ParryButton : MonoBehaviour
{
    ParryManager manager;

    Button button;
    CanvasGroup canvasGroup;

    float lifeTime = 1.5f;

    public void Init(ParryManager m)
    {
        manager = m;

        button = GetComponent<Button>();

        canvasGroup = gameObject.AddComponent<CanvasGroup>();

        button.onClick.AddListener(OnClick);

        StartCoroutine(FadeAndDie());
    }

    void OnClick()
    {
        manager.OnSuccess();

        Destroy(gameObject);
    }

    IEnumerator FadeAndDie()
    {
        float timer = 0;

        while (timer < lifeTime)
        {
            timer += Time.deltaTime;

            canvasGroup.alpha = 1 - (timer / lifeTime);

            yield return null;
        }

        manager.OnFail();

        Destroy(gameObject);
    }
}