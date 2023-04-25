using UnityEngine;

namespace GameProject
{
	public interface IAudioPlayer
	{
		AudioSource Source { get; }
		void Play(AudioClip clip, float volume = 1, bool loop = false);
		float Progress { get; }
		bool Active { get; }
		void Play();
		void Stop();
		void Pause(bool pause);
	}
}