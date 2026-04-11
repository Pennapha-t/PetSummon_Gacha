using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CatSelector : MonoBehaviour, IPointerClickHandler
{
    public int catID;

    private Image img;
    private Color originalColor;
    private Vector3 originalScale;

    private static CatSelector lastSelected;

    void Start()
    {
        img = GetComponent<Image>();
        originalScale = transform.localScale;

        if (img != null)
        {
            originalColor = img.color;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (lastSelected != null && lastSelected != this)
        {
            lastSelected.ResetVisual();
        }

        if (img != null)
        {
            img.color = new Color(1.2f, 1.2f, 1.2f);
            transform.localScale = originalScale * 1.15f;
        }

        lastSelected = this;

        if (CompareTag("Player"))
            BattleManager.Instance.SetSelectedPlayer(GetComponent<PlayerStats>());
        else if (CompareTag("Enemy"))
            BattleManager.Instance.SetSelectedTarget(GetComponent<EnemyStats>());

        //Debug.Log("Clicked: " + gameObject.name);
    }

    public void ResetVisual()
    {
        if (img != null)
        {
            img.color = originalColor;
            transform.localScale = originalScale;
        }
    }
}