using System;
using UnityEngine;

namespace Sounds
{
    public class SFX_Pool : PoolBase<AudioSource>
    {
        public static SFX_Pool Instance = null;

        protected override void Singleton()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else 
            {
                Destroy(this);
            }
        }

        public void Play(AudioClip clip)
        {
            if(clip != null)
            {
                var item = GetPoolItem();
                item.clip = clip;
                item.Play();
            }
        }

        protected override bool CheckItem(AudioSource item)
        {
            return !item.isPlaying;
        }
    }
}