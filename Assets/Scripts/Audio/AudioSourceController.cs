using System;
using System.Collections;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    public event Action OnClipEnded;
    
    public void Initialize(AudioClip clip, bool loop, float volume)
    {
        _audioSource.loop = loop;
        _audioSource.clip = clip;
        _audioSource.volume = volume;
    }

    public void Play()
    {
        _audioSource.Play();
        _isPlaying = true;
        
        if (_audioSource.loop) return;
        var clipLength = _audioSource.clip.length;
        StartCoroutine(ThrowOnClipEnded(clipLength));
    }

    public void Stop()
    {
        if (_audioSource != null)
        {
            _audioSource.Stop();
        }
        _isPlaying = false;
    }

    public bool IsPlaying()
    {
        return _isPlaying;
    }
    
    private AudioSource _audioSource;
    private bool _isPlaying = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    private IEnumerator ThrowOnClipEnded(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        OnClipEnded?.Invoke();
        _isPlaying = false;
    }
}
