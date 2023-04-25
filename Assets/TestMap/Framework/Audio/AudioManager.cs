using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace GameProject
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        #region ----字段----
        private float volume = 1;
        private readonly Dictionary<string, List<IAudioPlayer>> audios = new Dictionary<string, List<IAudioPlayer>>();
        public const string AudioPath = "Audio/";
        #endregion

        #region ----属性----
        public float Volume
        {
            get => volume;
            set
            {
                float vTemp = Mathf.Clamp(value, 0.0001f, 1);
                float temp = vTemp / volume;
                foreach (var auList in audios.Values)
                {
                    foreach (var au in auList)
                    {
                        au.Source.volume = au.Source.volume * temp;
                    }
                }
                volume = vTemp;
            }
        }
        #endregion

        #region ----公有方法----
        public IAudioPlayer Play(string name, float volume = 1, bool loop = false)
        {
            if (TryGetAudio(name, out var au))
            {
                au.Source.volume = volume * this.volume;
                au.Source.loop = loop;
                au.Play();
                return au;
            }
            else
            {
                if (TryLoadAudio(name, out var clip))
                {
                    IAudioPlayer ap = CreateAudio();
                    ap.Play(clip, this.volume * volume, loop);
                    if (!audios.ContainsKey(name))
                    {
                        List<IAudioPlayer> li = new List<IAudioPlayer>();
                        li.Add(ap);
                        audios.Add(name, li);
                    }
                    else
                    {
                        audios[name].Add(ap);
                    }
                    return ap;
                }
                return null;
            }
        }

        public void StopAll()
        {
            foreach (var auList in audios.Values)
            {
                foreach (var au in auList)
                {
                    au.Stop();
                }
            }
        }
        #endregion

        #region ----私有方法----
        private bool TryLoadAudio(string name, out AudioClip clip)
        {
            clip = Resources.Load<AudioClip>(AudioPath + name);
            if (clip == null)
            {
                return false;
            }

            return true;
        }

        private bool TryGetAudio(string name, out IAudioPlayer player)
        {
            player = null;
            if (audios.TryGetValue(name, out var list))
            {
                if (list.Count > 0)
                {
                    foreach (var au in list)
                    {
                        if (!au.Active)
                        {
                            player = au;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private AudioPlayer CreateAudio()
        {
            GameObject go = new GameObject("AudioPlayer");
            go.transform.SetParent(transform);
            AudioSource source = go.AddComponent<AudioSource>();
            source.playOnAwake = false;
            return new AudioPlayer(source);
        }
        #endregion

    }
}