using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioInstance> _audioList;
    [SerializeField] private AudioSourceController _sourcePrefab;
        
    private List<AudioSourceController> _allSources = new List<AudioSourceController>();

    public void PlayAudio(string audioUid)
    {
        var source = PickSource();

        var audioInstance = _audioList.FirstOrDefault(item => item.Uid == audioUid);
        if (audioInstance == null)
        {
            Debug.LogError($"Can't find audio with uid: {audioUid}");
            return;
        }
        
        audioInstance.Play(source);
    }
    
    public void StopAudio(string audioUid)
    {
        var audioInstance = _audioList.FirstOrDefault(item => item.Uid == audioUid);
        if (audioInstance == null)
        {
            Debug.LogError($"Can't find audio with uid: {audioUid}");
            return;
        }
        
        audioInstance.Stop();
    }

    private void Awake()
    {
        foreach (var audioInstance in _audioList)
        {
            audioInstance.Init();
        }
    }

    private AudioSourceController PickSource()
    {
        AudioSourceController vacantSource = null;
        foreach (var source in _allSources)
        {
            if (source.IsPlaying()) continue;
            vacantSource = source;
        }
        if (vacantSource != null) return vacantSource;
            
        vacantSource = Instantiate(_sourcePrefab, transform);
        _allSources.Add(vacantSource);
        return vacantSource;
    }
}