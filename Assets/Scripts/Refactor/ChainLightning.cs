using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ChainLightning : MonoBehaviour
{
    Transform P4;
    [SerializeField] SphereCollider col;
    [SerializeField] Animator anim;
    [SerializeField] VisualEffect effect;

    private void Start()
    {
        //anim = GetComponent<Animator>();
        //col = GetComponent<SphereCollider>();
        gameObject.SetActive(false);
        //col.enabled = false;
        //effect = GetComponentInChildren<VisualEffect>();
        effect.gameObject.SetActive(false);

    }


    void SetEndPosition(Vector3 endPos)
    {
        P4.position = endPos;
    }

    public void Expand()
    {
        gameObject.SetActive(true);
        //anim.SetTrigger("ExpandTrigger");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) SetEndPosition(other.transform.position);
        col.enabled = false;
        effect.gameObject.SetActive(true);
    }
}
