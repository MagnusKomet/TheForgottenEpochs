using UnityEngine;
using System.Collections;

namespace SlimUI.ModernMenu{
    public class CheckMusicVolume : MonoBehaviour
    {
        private AudioSource[] audioSources;

        public void Start()
        {
            audioSources = GetComponents<AudioSource>();
            UpdateVolume();
        }

        public void UpdateVolume()
        {           

            if(audioSources != null)
            {
                float volume = PlayerPrefs.GetFloat("MusicVolume");
            
                foreach (AudioSource source in audioSources)
                {
                    source.volume = volume;
                }
            }
            
        }
    }

}