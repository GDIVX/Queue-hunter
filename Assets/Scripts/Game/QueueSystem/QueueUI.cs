using System.Collections.Generic;
using Scirpts;
using UnityEngine;
using UnityEngine.UI;

public class QueueUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private MarbelUI _marbelUI;
    [SerializeField] private Transform _parent;
    [SerializeField] private QueueSystem _queueSystem;

    private List<MarbelUI> _marbelUis = new List<MarbelUI>();
    private float _imageHeight;
    
    void Start()
    {
        _imageHeight = _image.rectTransform.rect.height - 100;
        _queueSystem.OnShot += RemoveUI;
    }

    public void AddNewUiMarbel(Morbel morbel)
    {
        MarbelUI marbelUI = Instantiate(_marbelUI,_parent);
        marbelUI.Init(morbel);
        RectTransform rectTransform = (RectTransform)marbelUI.transform;

        _marbelUis.Add(marbelUI);
    }

    public void RemoveUI(int id)
    {
        foreach (var marbelUi in _marbelUis)    
        {
            if (marbelUi.Id == id)
            {
                Destroy(marbelUi.gameObject);
                _marbelUis.Remove(marbelUi);
                return;
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < _marbelUis.Count; i++)
        {
            RectTransform rect = (RectTransform)_marbelUis[i].transform;

            float dis = (_marbelUis[i].DisPro * _imageHeight) / 100;
            
            rect.anchoredPosition = new Vector3(0, -dis);
        }
    }
}
