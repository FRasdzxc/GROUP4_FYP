using System;
using System.Linq;
using UnityEngine;

namespace PathOfHero.Controllers
{
    [CreateAssetMenu(fileName = "CursorController", menuName = "Path of Hero/Controller/Cursor")]
    public class CursorController : ScriptableObject
    {
        public enum CursorType
        {
            Default,
            Crosshair
        }

        [SerializeField]
        private CursorProfile[] m_Profiles;

        public void ChangeCursor(CursorType type)
        {
            var profile = m_Profiles.First(p => p.type == type);
            if (profile == null)
            {
                Debug.LogWarning($"[Cursor Contoller] Missing profile for type '{type}'");
                return;
            }

            Cursor.SetCursor(profile.texture, profile.hotspot, CursorMode.Auto);
        }

        [Serializable]
        public class CursorProfile
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
    }
}
