using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using TMPro;
using Queue.Systems.UISystem;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI damageNumber;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] RectTransform canvasRectTransform;
    [SerializeField] float apearanceTime;
    public bool isAvailable = true;

    void Start()
    {
        //damageNumber = GetComponent<TextMeshProUGUI>();
        isAvailable = true;
    }

    public void ActivateDamageNumber(float num, Vector3 pos)
    {
        Debug.Log($"Displaying damage number {num}");
        isAvailable = false;
        // get the screen position

        //Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);

        //// convert the screen position to the local anchored position

        //Vector2 anchoredPosition = transform.InverseTransformPoint(screenPoint);

        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(pos);
        Vector2 worldObjectScreenPosition = new Vector2(
           ((viewportPosition.x * canvasRectTransform.sizeDelta.x) - (canvasRectTransform.sizeDelta.x * 0.5f)),
           ((viewportPosition.y * canvasRectTransform.sizeDelta.y) - (canvasRectTransform.sizeDelta.y * 0.5f)));

        rectTransform.anchoredPosition = worldObjectScreenPosition;
        damageNumber.text = num.ToString();
        StartCoroutine(DisplayDamageNumbers());

    }

    private IEnumerator DisplayDamageNumbers()
    {
        yield return new WaitForSeconds(apearanceTime);
        isAvailable = true;
        gameObject.SetActive(false);
    }
}
