using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WrongClickAudioInstance", menuName = "AudioInstances/Wrong Click AudioInstance")]
public class WrongClickAudioInstance : AudioInstance
{
    [SerializeField] private List<AudioClip> _clips;

    public override void Play(AudioSourceController source)
    {
        var randomIdx = Random.Range(0, _clips.Count);
        var clip = _clips[randomIdx];
        
        source.Initialize(clip, _loop, _volume);
        
        Sources.Add(source);
        _onEndHandlers.Add(source, () => RemoveSource(source));
        source.OnClipEnded += _onEndHandlers[source];
        
        source.Play();
    }
}
