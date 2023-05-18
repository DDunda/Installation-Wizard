using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IAudioManager
{
	public AudioSource[] PlayClips(AudioClip[] clips, Vector2 position, string name = "Audio Source");
	public AudioSource[] PlayClips(AudioClip[] clips, string name = "Audio Source");

	public AudioSource PlayClip(AudioClip clip, Vector2 position, string name = "Audio Source");
	public AudioSource PlayClip(AudioClip clip, string name = "Audio Source");
}

public class AudioManager : IAudioManager
{
	public static AudioManager instance { get; private set; } = new();
	private GameObject _audioSourceFolder = null;

	public AudioManager() => _audioSourceFolder = GameObject.FindGameObjectWithTag("AudioFolder");

	public AudioManager(GameObject folder) => _audioSourceFolder = folder;

	public AudioSource[] PlayClips(AudioClip[] clips, Vector2 position, string name = "Audio Source")
	{
		if (clips.All(c => c == null)) return null;

		GameObject go = new GameObject(name);
		go.transform.position = position;
		go.transform.parent = _audioSourceFolder.transform;

		List<AudioSource> sources = new(clips.Length);

		foreach (AudioClip c in clips)
		{
			if (c == null) continue;

			AudioSource a = go.AddComponent<AudioSource>();
			a.clip = c;
			a.Play();

			sources.Add(a);
		}

		var d = go.AddComponent<AudioDeleter>();
		d.audioSources = sources.ToArray();

		return d.audioSources;
	}

	public AudioSource[] PlayClips(AudioClip[] clips, string name = "Audio Source") => PlayClips(clips, Vector2.zero, name);

	public AudioSource PlayClip(AudioClip clip, Vector2 position, string name = "Audio Source")
	{
		if (clip == null) return null;

		GameObject go = new GameObject(name);
		go.transform.position = position;
		go.transform.parent = _audioSourceFolder.transform;

		AudioSource a = go.AddComponent<AudioSource>();
		a.clip = clip;
		a.Play();

		var d = go.AddComponent<AudioDeleter>();
		d.audioSources = new[] { a };

		return a;
	}

	public AudioSource PlayClip(AudioClip clip, string name = "Audio Source") => PlayClip(clip, Vector2.zero, name);
}

public class AudioManagerComponent : MonoBehaviour, IAudioManager
{
	[SerializeField] private GameObject _audioSourceFolder;
	[SerializeField] private AudioManager _audioManager;

	private void Awake()
	{
		_audioManager = new(_audioSourceFolder);
	}

	public AudioSource[] PlayClips(AudioClip[] clips, Vector2 position, string name = "Audio Source") => _audioManager.PlayClips(clips, position, name);
	public AudioSource[] PlayClips(AudioClip[] clips, string name = "Audio Source") => _audioManager.PlayClips(clips, name);
	public AudioSource PlayClip(AudioClip clip, Vector2 position, string name = "Audio Source") => _audioManager.PlayClip(clip, position, name);
	public AudioSource PlayClip(AudioClip clip, string name = "Audio Source") => _audioManager.PlayClip(clip, name);
}
