using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using PathOfHero.Utilities;

namespace PathOfHero.Controllers
{
    public class PostProcessController : Singleton<PostProcessController>
    {
        public enum ProfileType
        {
            Default,
            Death
        }

        [SerializeField]
        private PostProcessProfile[] m_Profiles;

        private Dictionary<ProfileType, Volume> m_Volumes;
        private ProfileType m_Current;

        protected override void Awake()
        {
            base.Awake();
            m_Volumes = new();
            foreach (var entry in m_Profiles)
            {
                var volume = gameObject.AddComponent<Volume>();
                volume.weight = 0f;
                volume.sharedProfile = entry.volumeProfile;
                m_Volumes.Add(entry.type, volume);
            }
        }

        private void Start()
        {
            if (!m_Volumes.ContainsKey(ProfileType.Default))
            {
                Debug.LogWarning("[Post Process Controller] Default profile not found!");
                return;
            }

            m_Volumes[ProfileType.Default].weight = 1.0f;
            m_Current = ProfileType.Default;
        }

        public void ChangeVolume(ProfileType type, bool animated = true)
        {
            if (!m_Volumes.ContainsKey(type))
            {
                Debug.LogWarning($"[Post Process Controller] Missing profile for type '{type}'. Change aborted.");
                return;
            }

            if (m_Current == type)
                return;

            var oldVolume = m_Volumes[m_Current];
            var newVolume = m_Volumes[type];
            if (animated)
            {
                DOTween.To(() => oldVolume.weight, v => oldVolume.weight = v, 0f, 1f).SetEase(Ease.InQuart);
                DOTween.To(() => newVolume.weight, v => newVolume.weight = v, 1f, 1f).SetEase(Ease.OutQuart);
            }
            else
            {
                oldVolume.weight = 0f;
                newVolume.weight = 1f;
            }
            m_Current = type;
        }

        [Serializable]
        public class PostProcessProfile
        {
            public ProfileType type;
            public VolumeProfile volumeProfile;
        }
    } 
}
