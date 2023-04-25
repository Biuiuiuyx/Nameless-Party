using UnityEngine;
using Framework;

namespace GameProject
{
	public class AudioPlayer : IAudioPlayer
	{
        #region ----字段----
        private AudioSource source;
        private bool pause;
        #endregion

        #region ----构造方法----
        public AudioPlayer(AudioSource source)
        {
            this.source = source;
        }
        #endregion

        #region ----实现IAudioPlayer----
        AudioSource IAudioPlayer.Source => source;
        float IAudioPlayer.Progress => source == null ? 0 : source.time / source.clip.length;
        bool IAudioPlayer.Active => source != null && source.isPlaying;
        void IAudioPlayer.Play(AudioClip clip, float volume, bool loop)
        {
            source.gameObject.SetActive(true);
            source.clip = clip;
            pause = false;
            source.volume = volume;
            source.loop = loop;
            source.Play();
        }

        void IAudioPlayer.Play()
        {
            if (pause)
            {
                UnPause();
                return;
            }
            if (source != null)
            {
                source.time = 0;
                source.Play();
            }
        }

        void IAudioPlayer.Stop()
        {
            source?.Stop();
        }

        void IAudioPlayer.Pause(bool pause)
        {
            if (this.pause != pause)
            {
                if (pause)
                    Pause();
                else
                    UnPause();
            }
        }
        #endregion

        #region ----私有方法----
        private void Pause()
        {
            source?.Pause();
            pause = true;
        }
        private void UnPause()
        {
            source?.UnPause();
            pause = false;
        }
        #endregion
    }
}