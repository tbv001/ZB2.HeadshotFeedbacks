using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

namespace HeadshotFeedbacks.Components;

public class AudioLoader : MonoBehaviour
{
    private static AudioSource _audioSource;

    private void Awake()
    {
        var path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException(),
            "Sounds", "Headshot.mp3");
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = LoadAudio(path);
    }

    private AudioClip LoadAudio(string path)
    {
        if (!File.Exists(path))
        {
            HeadshotFeedbacks.Logger.LogError($"Audio file not found at: {path}");
            return null;
        }

        var url = "file://" + path;
        using var uwr = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
        uwr.SendWebRequest();

        while (!uwr.isDone)
        {
        }

        if (uwr.result == UnityWebRequest.Result.Success)
            return DownloadHandlerAudioClip.GetContent(uwr);

        HeadshotFeedbacks.Logger.LogError($"Failed to load audio: {uwr.error}");
        return null;
    }

    public static void PlaySfx()
    {
        if (_audioSource.clip == null) return;

        if (PersistenceController.instance == null)
        {
            _audioSource.volume = 0.5f;
            _audioSource.PlayOneShot(_audioSource.clip);
            return;
        }

        var master = PersistenceController.instance.soundsMenu.saveAudio.master / 100f;
        var sfx = PersistenceController.instance.soundsMenu.saveAudio.sfx / 100f;
        _audioSource.volume = 0.5f * master * sfx;
        _audioSource.PlayOneShot(_audioSource.clip);
    }
}
