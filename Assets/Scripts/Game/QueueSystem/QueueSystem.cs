using System;
using System.Collections.Generic;
using Scirpts;
using UnityEngine;

public class QueueSystem : MonoBehaviour, ILoader
{
    public event Action<int> OnShot;
    
    public const float MorbelSize = 0.5f;

    [SerializeField] private QueueUI _queueUI;
    
    [SerializeField] private MorbelBlockConfig _morbelBlockConfig;
    [SerializeField] private float _laodDis;
    
    private List<BaseLoadObject> _loadedMorbels = new List<BaseLoadObject>();
    private List<BaseLoadObject> _morbales = new List<BaseLoadObject>();

    private float _currentMorbletime = 0;
    private int _currentMorbleLoadingIndex = 0;

    private bool _doneBlock =false;
    private int _mirableId = 1;


    public float NumberOfLoadedObjects => _loadedMorbels.Count;
    public float MaxDistanceToTravel => _laodDis;
    public float FireRate => 1.5f;

    public float CurrentDistanceToTravel    
    {
        get
        {
            float currentDis = _laodDis;

            for (int i = 0; i < _loadedMorbels.Count; i++)
            {
                currentDis -= MorbelSize;
            }

            return currentDis;
        }
    }

    public float GetDistanceByLoadedIndex(int index) =>
        MaxDistanceToTravel - MorbelSize * index;
    
    
    public void LoadNewBlock(MorbelBlockConfig morbelBlockConfig)
    {
        _currentMorbletime = 0;
        _currentMorbleLoadingIndex = 0;
    }

    public void AddMorble(MorbaleConfig morbaleConfig)
    {
        var morble = new Morbel(morbaleConfig,this, _mirableId);
        _morbales.Add(morble);
        //Debug.Log($"Start dissenting at speed: {morbaleConfig.speed}, id{_mirableId}");
        _mirableId++;
        _queueUI.AddNewUiMarbel(morble);
    }

    public bool GetMorbole(out BaseLoadObject morbel)
    {
        if (_loadedMorbels.Count == 0)
        {
            morbel = null;
            return false;
        }

        morbel = _loadedMorbels[0];

        if (!morbel.IsReady)
            return false;

        _morbales.Remove(morbel);
        _loadedMorbels.Remove(morbel);

        for (int i = 0; i < _loadedMorbels.Count; i++)
            _loadedMorbels[i].LoadedIndex = i;
        
        OnShot?.Invoke(morbel.Id);
        return true;
    }

    private void Update()
    {
        for (int i = 0; i < _morbales.Count; i++)
        {
            BaseLoadObject morbel = _morbales[i];

            morbel.UpdateTravelStatus();
            
            if (morbel.IsLoaded && !_loadedMorbels.Contains(morbel))
            {
                _loadedMorbels.Add(morbel);
                morbel.OnLoaded(_loadedMorbels.Count - 1);
                Debug.Log($"Loaded new morble Id: {morbel.Id}");
            }
        }

        if (_doneBlock)
            return;
        
        MorbleDisntConfig currentMorbleDisntConfig = _morbelBlockConfig.MorbleDisntConfig[_currentMorbleLoadingIndex];

        _currentMorbletime += Time.deltaTime;

        if (_currentMorbletime >= currentMorbleDisntConfig.Delay)
        {
            _currentMorbletime = 0;
            AddMorble(currentMorbleDisntConfig.morbaleConfig);
            
            _currentMorbleLoadingIndex++;
            
            if (_currentMorbleLoadingIndex == _morbelBlockConfig.MorbleDisntConfig.Length)
                _doneBlock = true;
        }
    }
}

public interface ILoader
{
    public event Action<int> OnShot; 
    public float NumberOfLoadedObjects { get; }
    public float MaxDistanceToTravel { get; }
    public float CurrentDistanceToTravel { get; }
    public float FireRate { get; }
    public float GetDistanceByLoadedIndex(int index);
}
