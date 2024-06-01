using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using TMPro;
using Queue.Systems.UISystem;
using DG.Tweening;


public class DamageNumber : MonoBehaviour
{
    [SerializeField] TextMeshPro damageNumber;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] RectTransform canvasRectTransform;
    [SerializeField] float apearanceTime;
    public bool isAvailable = true;

    void Start()
    {
        isAvailable = true;
    }

    public void ActivateDamageNumber(float num, Vector3 pos)
    {
        Debug.Log($"Displaying damage number {num}");
        isAvailable = false;
        damageNumber.text = num.ToString();
        pos = new Vector3(pos.x, pos.y + 5, pos.z);
        rectTransform.position = pos;
        StartCoroutine(DisplayDamageNumbers());

    }

    private IEnumerator DisplayDamageNumbers()
    {
        transform.DOMoveY(10, apearanceTime);
        transform.DOScale(.7f, apearanceTime / 2);
        yield return new WaitForSeconds(apearanceTime / 2);
        transform.DOScale(.3f, apearanceTime / 2);
        yield return new WaitForSeconds(apearanceTime);
        isAvailable = true;
        gameObject.SetActive(false);
    }
}
