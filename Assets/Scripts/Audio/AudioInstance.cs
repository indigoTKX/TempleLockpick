using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioInstance", menuName = "AudioInstances/Default AudioInstance")]
public class AudioInstance : ScriptableObject
{
    [SerializeField] public string Uid;
    [SerializeField] protected AudioClip _clip;
    [SerializeField] protected bool _loop;
    
    [Range(0,1)]
    [SerializeField] protected float _volume = 1;

    public void Init()
    {
        Sources.Clear();
        _onEndHandlers.Clear();
    }
    
    public virtual void Play(AudioSourceController source)
    {
        source.Initialize(_clip, _loop, _volume);
        
        Sources.Add(source);
        _onEndHandlers.Add(source, () => RemoveSource(source));
        source.OnClipEnded += _onEndHandlers[source];
        
        source.Play();
    }

    public virtual void Stop()
    {
        StopAll();
    }
    
    protected List<AudioSourceController> Sources = new();
    protected Dictionary<AudioSourceController, Action> _onEndHandlers = new();

    private void StopAll()
    {
        foreach (var source in Sources)
        {
            source.Stop();
            source.OnClipEnded -= _onEndHandlers[source];
            _onEndHandlers.Remove(source);
        }
        Sources.Clear();
        _onEndHandlers.Clear();
    }

    protected void RemoveSource(AudioSourceController source)
    {
        source.OnClipEnded -= _onEndHandlers[source];
        _onEndHandlers.Remove(source);
        Sources.Remove(source);
    }
}
