using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

namespace HeadshotFeedback.Components;

public class AudioLoader : MonoBehaviour
{
    private static readonly List<AudioClip> AudioClips = [];
    private static AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();

        var soundsDir = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException(),
            "Sounds");

        if (!Directory.Exists(soundsDir))
        {
            HeadshotFeedback.Logger.LogError($"Sounds directory not found at: {soundsDir}");
            return;
        }

        var files = Directory.GetFiles(soundsDir);
        foreach (var file in files)
        {
            var clip = LoadAudio(file);
            if (clip != null)
            {
                AudioClips.Add(clip);
            }
        }
    }

    private static AudioClip LoadAudio(string path)
    {
        if (!File.Exists(path))
        {
            HeadshotFeedback.Logger.LogError($"Audio file not found at: {path}");
            return null;
        }

        var url = $"file://{path}";
        using var uwr = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
        uwr.SendWebRequest();

        while (!uwr.isDone)
        {
        }

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            return DownloadHandlerAudioClip.GetContent(uwr);
        }

        HeadshotFeedback.Logger.LogError($"Failed to load audio: {uwr.error}");
        return null;
    }

    private static float GetVolume()
    {
        if (PersistenceController.instance == null)
            return 0.5f;

        var master = PersistenceController.instance.soundsMenu.saveAudio.master / 100f;
        var sfx = PersistenceController.instance.soundsMenu.saveAudio.sfx / 100f;
        return 0.5f * master * sfx;
    }

    public static void PlaySfx(Vector3? position = null)
    {
        if (AudioClips.Count == 0)
            return;

        var clip = AudioClips[UnityEngine.Random.Range(0, AudioClips.Count)];
        var volume = GetVolume();
        if (HeadshotFeedback.Use3DAudio.Value && position.HasValue)
        {
            AudioSource.PlayClipAtPoint(clip, position.Value, volume);
            return;
        }

        _audioSource.volume = volume;
        _audioSource.PlayOneShot(clip);
    }
}
