using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioDeleter : MonoBehaviour
{
	public AudioSource[] audioSources;

	void Update()
	{
		if (audioSources.All(s => !s.isPlaying))
			Destroy(gameObject);
	}
}
