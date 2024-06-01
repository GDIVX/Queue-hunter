using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using TMPro;
using Queue.Systems.UISystem;


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

    private void Update()
    {
        
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
        yield return new WaitForSeconds(apearanceTime);
        isAvailable = true;
        gameObject.SetActive(false);
    }
}
